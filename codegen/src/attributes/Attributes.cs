using System;
using CodeGeneration.Roslyn;

namespace Laconic.CodeGeneration
{
    [AttributeUsage(AttributeTargets.Interface)]
    [CodeGenerationAttribute("Laconic.CodeGeneration.UnionGenerator, Laconic.CodeGen.Generators")]
    public class UnionAttribute : Attribute
    {
    }
    
    [AttributeUsage(AttributeTargets.Interface)]
    [CodeGenerationAttribute("Laconic.CodeGeneration.RecordsGenerator, Laconic.CodeGen.Generators")]
    public class RecordsAttribute : Attribute
    {
    }
    
    [AttributeUsage(AttributeTargets.Interface)]
    [CodeGenerationAttribute("Laconic.CodeGeneration.SignalsGenerator, Laconic.CodeGen.Generators")]
    public class SignalsAttribute : Attribute
    {
    }
}
