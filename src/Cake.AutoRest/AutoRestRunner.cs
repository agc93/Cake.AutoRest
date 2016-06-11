using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.AutoRest
{
    public class AutoRestRunner : Cake.Core.Tooling.Tool<AutoRestSettings>
    {
        public AutoRestRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools, ICakeLog log) : base(fileSystem, environment, processRunner, tools)
        {
            Environment = environment;
            Log = log;

        }

        private ICakeLog Log { get; set; }

        private ICakeEnvironment Environment { get; set; }

        protected override string GetToolName() => "Azure AutoRest";

        protected override IEnumerable<string> GetToolExecutableNames()
        {
            yield return "AutoRest.exe";
            yield return "AutoRest";
        }

        public DirectoryPath Generate(FilePath inputFile, Action<AutoRestSettings> configure = null)
        {
            var settings = new AutoRestSettings(inputFile);
            configure?.Invoke(settings);
            return Generate(inputFile, settings);
        }

        public DirectoryPath Generate(FilePath inputFile, AutoRestSettings settings)
        {
            settings.InputFile = settings.InputFile ?? inputFile;
            var args = GetToolArguments(settings);
            Log.Verbose(args.Render());
            Run(settings, args);
            return settings.OutputDirectory ?? "./Generated";
        }

        private ProcessArgumentBuilder GetToolArguments(AutoRestSettings settings)
        {
            var args = new ProcessArgumentBuilder();
            settings.Build(args);
            return args;
        }
    }
}
