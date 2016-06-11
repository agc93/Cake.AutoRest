using System.Collections.Generic;

namespace Cake.AutoRest
{
    public interface IGeneratorSettings
    {
        CodeGenerator Generator { get; }
        Dictionary<string, string> GetArguments();
    }
}