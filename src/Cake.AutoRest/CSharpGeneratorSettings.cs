using System.Collections.Generic;

namespace Cake.AutoRest
{
    public class CSharpGeneratorSettings : IGeneratorSettings
    {
        public CodeGenerator Generator => CodeGenerator.CSharp;

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

        public bool? InternalConstructors { get; set; }
        public bool? UseDateTimeOffset { get; set; }
        public string SyncMethodMode { get; set; }
    }
}