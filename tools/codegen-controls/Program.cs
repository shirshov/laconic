global using Microsoft.Maui.Controls;
global using System;
global using System.Linq;
global using System.Collections.Generic;

using System.IO;
using System.Reflection;
using Microsoft.Maui.Controls.Compatibility;
using Layout = Microsoft.Maui.Controls.Layout;

namespace Laconic.CodeGen;

class Program
{
    static IEnumerable<Type> GetDirectDescendants(Type type) => type.Assembly.GetTypes()
        .Where(t => t.BaseType == type && t.IsPublic).OrderBy(t => t.Name);

    static void AddDirectDescendants(Type type, List<(Type, Type)> list)
    {
        foreach (var t in GetDirectDescendants(type))
        {
            list.Add((t, type));
            AddDirectDescendants(t, list);
        }
    }

    static IEnumerable<FieldInfo> GetProps(Type t) => t.GetFields(BindingFlags.Public | BindingFlags.Static)
        .Where(x => typeof(BindableProperty).IsAssignableFrom(x.FieldType))
        .Where(x => !Definitions.Defs[t].SkipGeneration
            .Contains(((BindableProperty) x.GetValue(null)).PropertyName))
        .Where(x => !((BindableProperty) x.GetValue(null)).IsReadOnly)
        .Where(x => !x.IsDefined(typeof(ObsoleteAttribute)))
        .OrderBy(x => x.Name);

    public static string GetDef()
    {
        var flatList = new List<(Type Type, Type Base)>();

        AddDirectDescendants(typeof(BindableObject), flatList);
        AddDirectDescendants(typeof(Layout<View>), flatList);
        AddDirectDescendants(typeof(MultiPage<Page>), flatList);

        var notIgnored = flatList
            .Where(x => Definitions.Defs.ContainsKey(x.Type)
                        && Definitions.Defs[x.Type] != Definitions.NotUsed
                        && Definitions.Defs[x.Type] != Definitions.WrittenManually
                        && Definitions.Defs[x.Type] != Definitions.NotImplemented)
            .OrderBy(x => x.Type.Name);

        static IEnumerable<EventInfo> GetEvents(Type type) => type
            .GetEvents(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
            .Where(e => !Definitions.Defs[type].SkipGeneration.Contains(e.Name))
            .OrderBy(x => x.Name);

        static IEnumerable<string> GenerateAll(IEnumerable<(Type Type, Type Base)> all)
        {
            static string WithXfPrefix(Type type)
            {
                var providedByLaconic = new[] {
                    "Color", "Thickness", "ImageSource", "Keyboard", "FormattedString", 
                    "CornerRadius", "Easing", "View", "Brush"
                };

                return type.Namespace == "Microsoft.Maui.Control" &&
                       !(providedByLaconic.Contains(type.Name) || type.IsEnum)
                    ? "xf." + type.Name
                    : type.Name;
            }

            foreach (var c in all.Select(x => x.Type))
            {
                yield return "public " + (c.IsAbstract ? "abstract " : "") + $"partial class {c.Name}"
                             + (Definitions.Defs[c].HasGenericParameter ? "<T>" : "")
                             + (Definitions.Defs[c].DoNotInherit ? "\n{" : $" : View<xf.{c.Name}>\n{{");
                foreach (var p in GetProps(c))
                {
                    var bindableProperty = (BindableProperty) p.GetValue(null);
                    var propName = bindableProperty.PropertyName;
                    var propType = bindableProperty.ReturnType;
                    yield return $"    public {WithXfPrefix(propType)} {propName}";
                    yield return  "    {";
                    yield return $"        internal get => GetValue<{WithXfPrefix(propType)}>(xf.{c.Name}.{p.Name});";
                    yield return $"        init => SetValue(xf.{c.Name}.{p.Name}, value);";
                    yield return  "    }";
                }

                foreach (var e in GetEvents(c))
                {
                    var type = e.EventHandlerType;
                    var genericParam = "";
                    var genericEventHandlers = type.GenericTypeArguments;
                    if (genericEventHandlers.Length > 0)
                    {
                        genericParam += "xf." + (genericEventHandlers[0].DeclaringType == null
                            ? ""
                            : genericEventHandlers[0].Name + ".");
                        genericParam += type.Name + ",";
                    }

                    yield return $"    public Func<{genericParam}Signal> {e.Name}";
                    yield return  "    {";
                    yield return $"        init => SetEvent(nameof({e.Name}), value, (ctl, handler) => ctl.{e.Name} += handler, (ctl, handler) => ctl.{e.Name} -= handler);";
                    yield return  "    }";
                }
                yield return "}\n";
            }
        }

        return String.Join("\n", GenerateAll(notIgnored).ToArray());
    }

    static void Main(string[] _)
    {
        var s = "#nullable enable\n"
                + "using System.Net;\n"
                + "using Laconic.Shapes;\n"
                + "// ReSharper disable all\n\n"
                + "namespace Laconic;\n\n";

        s += GetDef();
        
        File.WriteAllText("../../src/Controls.Generated.cs", s);

        var rep = Report();
        Console.WriteLine(rep);
        File.WriteAllText("../../binding-report.md", rep);
    }

    static string Report()
    {
        var flatList = new List<(Type Type, Type Base)>();

        AddDirectDescendants(typeof(BindableObject), flatList);
        AddDirectDescendants(typeof(Layout), flatList);

        var res = "";

        // res += "## WRITTEN MANUALLY\n\n";
        // res += flatList
        //     .Where(x => Definitions.Defs.ContainsKey(x.Type) && Definitions.Defs[x.Type] == Definitions.WrittenManually)
        //     .Select(x => x.Type.Name)
        //     .OrderBy(x => x)
        //     .Aggregate("", (c, n) => c + n + "\n\n");

        res += "## Not Used\n\n";
        res += flatList
            .Where(x => Definitions.Defs.ContainsKey(x.Type) && Definitions.Defs[x.Type] == Definitions.NotUsed)
            .Select(x => x.Type.Name)
            .OrderBy(x => x)
            .Aggregate("", (c, n) => c + n + "\n\n");

        res += "## Not Implemented\n\n";
        res += flatList
            .Where(x => Definitions.Defs.ContainsKey(x.Type) && Definitions.Defs[x.Type] == Definitions.NotImplemented)
            .Select(x => x.Type.Name)
            .OrderBy(x => x)
            .Aggregate("", (c, n) => c + n + "\n\n");

        var undefined = flatList
            .Where(x => ! Definitions.Defs.ContainsKey(x.Type))
            .Select(x => x.Type.Name)
            .OrderBy(x => x);
        if (undefined.Any()) {
            res += "## Undefined\n\n";
            res += undefined.Aggregate("", (c, n) => c + n + "\n\n");
        }

        return res;
    }
}