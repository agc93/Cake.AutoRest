using System.Collections.Generic;

namespace Cake.AutoRest
{
    /// <summary>
    /// Used to define generator-specific settings, as with the <see cref="CSharpGeneratorSettings"/> class
    /// </summary>
    public interface IGeneratorSettings
    {
        /// <summary>
        /// The standard generator to use with a given generator settings instance
        /// </summary>
        CodeGenerator Generator { get; }
        /// <summary>
        /// Gets the arguments from the generator-specific settings
        /// </summary>
        /// <returns>The arguments in key/value pairs</returns>
        Dictionary<string, string> GetArguments();
    }
}