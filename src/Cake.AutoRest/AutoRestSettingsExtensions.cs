using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core.IO;

namespace Cake.AutoRest
{
    public static class AutoRestSettingsExtensions
    {
        public static AutoRestSettings UseNamespace(this AutoRestSettings settings, string ns)
        {
            settings.Namespace = ns;
            return settings;
        }

        public static AutoRestSettings OutputToDirectory(this AutoRestSettings settings, DirectoryPath path)
        {
            settings.OutputDirectory = path.FullPath;
            return settings;
        }

        public static AutoRestSettings WithGenerator(this AutoRestSettings settings, CodeGenerator gen)
        {
            settings.Generator = gen;
            return settings;
        }

        public static AutoRestSettings WithModeler(this AutoRestSettings settings, string modeler)
        {
            settings.Modeler = modeler;
            return settings;
        }

        public static AutoRestSettings UseClientName(this AutoRestSettings settings, string clientName)
        {
            settings.ClientName = clientName;
            return settings;
        }

        public static AutoRestSettings UseFlattenThreshold(this AutoRestSettings settings, int threshold)
        {
            settings.PayloadFlattenThreshold = threshold;
            return settings;
        }

        public static AutoRestSettings AddHeaderComment(this AutoRestSettings settings, string comment)
        {
            settings.HeaderComment = comment.Trim('/', '#', '*').Trim(); //just remove common comment delimiters
            return settings;
        }

        public static AutoRestSettings AddCredentials(this AutoRestSettings settings, bool useCredentials = true)
        {
            settings.AddCredentials = useCredentials;
            return settings;
        }

        public static AutoRestSettings OutputToFile(this AutoRestSettings settings, FilePath path)
        {
            settings.OutputFileName = path.FullPath;
            return settings;
        }

        public static AutoRestSettings WithVerboseOutput(this AutoRestSettings settings)
        {
            settings.Verbose = true;
            return settings;
        }

    }
}
