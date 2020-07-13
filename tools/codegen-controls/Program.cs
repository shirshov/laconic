using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace Laconic.CodeGen
{
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

            var notIgnored = flatList
                .Where(x => Definitions.Defs.ContainsKey(x.Type)
                            && Definitions.Defs[x.Type] != Definitions.NotUsed
                            && Definitions.Defs[x.Type] != Definitions.WrittenManually)
                .OrderBy(x => x.Type.Name);

            static IEnumerable<EventInfo> GetEvents(Type type) => type
                .GetEvents(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
                .Where(e => !Definitions.Defs[type].SkipGeneration.Contains(e.Name))
                .OrderBy(x => x.Name);

            static IEnumerable<string> GenerateAll(IEnumerable<(Type Type, Type Base)> all)
            {
                static string WithXfPrefix(Type type) =>
                    type.Namespace == "Xamarin.Forms" ? "xf." + type.Name : type.Name;

                foreach (var c in all.Select(x => x.Type))
                {
                    yield return "public " + (c.IsAbstract ? "abstract " : "") + $"partial class {c.Name}"
                                 + (Definitions.Defs[c].HasGenericParameter ? "<T>" : "")
                                 + (Definitions.Defs[c].DoNotInherit ? " {" : $" : View<xf.{c.Name}> {{");
                    foreach (var p in GetProps(c))
                    {
                        var bindableProperty = (BindableProperty) p.GetValue(null);
                        var propName = bindableProperty.PropertyName;
                        var propType = bindableProperty.ReturnType;
                        yield return $"    public {WithXfPrefix(propType)} {propName} {{";
                        yield return $"       get => GetValue<{WithXfPrefix(propType)}>(xf.{c.Name}.{p.Name});";
                        yield return $"       set => SetValue(xf.{c.Name}.{p.Name}, value);";
                        yield return "    }";
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

                        // var parameter = genTypes.Length == 0 ? "" : $"xf.{genTypes[0].DeclaringType == null ? "+", } {genTypes[0].Name}, ";
                        yield return $"    public Func<{genericParam}Signal> {e.Name} {{";
                        //yield return $"       get => (Expression<Func<{genericParam}Signal>>)Events[nameof({e.Name})];";
                        yield return
                            $"       set => SetEvent(nameof({e.Name}), value, (ctl, handler) => ctl.{e.Name} += handler, (ctl, handler) => ctl.{e.Name} -= handler);";
                        yield return "    }";
                    }

                    yield return "}\n";
                }
            }

            return String.Join("\n", GenerateAll(notIgnored).ToArray());
        }

        static void Main(string[] args)
        {
            var s = "#nullable enable\n"
                    + "using System;\n"
                    + "using System.Collections;\n"
                    + "using xf = Xamarin.Forms;\n"
                    + "using Laconic.Shapes;\n"
                    + "// ReSharper disable all\n\n"
                    + "namespace Laconic \n{";

            s += GetDef();

            s += "}";

            File.WriteAllText("../../src/Controls.Generated.cs", s);
        }

        static void PrintDict(IEnumerable<(Type Type, Type Base)> items)
        {
            foreach (var el in items.OrderBy(x => x.Type.Name))
                Console.WriteLine($" [typeof({el.Type.Name})] = NotUsed,");
        }
    }
}