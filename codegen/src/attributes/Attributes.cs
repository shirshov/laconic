using System;
using CodeGeneration.Roslyn;

namespace Laconic.CodeGen
{
    [AttributeUsage(AttributeTargets.Interface)]
    [CodeGenerationAttribute("Laconic.CodeGen.UnionGenerator, Laconic.CodeGen.Generators")]
    public class UnionAttribute : Attribute
    {
    }
    
    [AttributeUsage(AttributeTargets.Interface)]
    [CodeGenerationAttribute("Laconic.CodeGen.RecordsGenerator, Laconic.CodeGen.Generators")]
    public class RecordsAttribute : Attribute
    {
    }
}
