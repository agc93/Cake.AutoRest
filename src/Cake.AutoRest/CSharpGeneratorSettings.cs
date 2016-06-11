using System.Collections.Generic;

namespace Cake.AutoRest
{
    /// <summary>
    /// CSharp generator-specific settings for use when generating C# clients
    /// </summary>
    public class CSharpGeneratorSettings : IGeneratorSettings
    {
        /// <summary>
        /// The standard generator to use. Can still be overridden by <see cref="AutoRestSettings"/>
        /// </summary>
        public CodeGenerator Generator => CodeGenerator.CSharp;

        /// <summary>
        /// Gets the arguments to use when invoking
        /// </summary>
        /// <returns>key/value pairs of arguments and values for set parameters</returns>
        public Dictionary<string, string> GetArguments()
        {
            var dict = new Dictionary<string, string>();
            if (InternalConstructors.HasValue)
            {
                dict.Add("InternalConstructors", InternalConstructors.Value.ToString().ToLower());
            }
            if (UseDateTimeOffset.HasValue)
            {
                dict.Add("UseDateTimeOffset", UseDateTimeOffset.Value.ToString().ToLower());
            }
            if (!string.IsNullOrWhiteSpace(SyncMethodMode))
            {
                dict.Add("SyncMethods", SyncMethodMode);
            }
            return dict;
        }

        /// <summary>
        /// Indicates whether ctor needs to be generated with internal protection level.
        /// </summary>
        public bool? InternalConstructors { get; set; }
        /// <summary>
        ///  Indicates whether to use DateTimeOffset instead of DateTime to model date-time types.
        /// </summary>
        public bool? UseDateTimeOffset { get; set; }
        /// <summary>
        /// Specifies mode for generating sync wrappers. 
        /// <para>Supported value are Essential - generates only one sync returning body or header (default), All - generates one sync method for each async method, and None - does not generate any sync methods</para>
        /// </summary>
        public string SyncMethodMode { get; set; }
    }
}