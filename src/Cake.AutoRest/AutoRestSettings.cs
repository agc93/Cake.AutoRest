using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.IO.Arguments;
using Cake.Core.Tooling;

namespace Cake.AutoRest
{
    public class AutoRestSettings : ToolSettings
    {
        public AutoRestSettings(FilePath inputFile)
        {
            InputFile = inputFile;
        }

        public AutoRestSettings()
        {
            
        }

        private FilePath InputFile { get; set; }

        // private ICakeEnvironment Environment { get; set; }

        public IGeneratorSettings GeneratorSettings { get; set; }
        public string Namespace { internal get; set; }
        public DirectoryPath OutputDirectory { internal get; set; }
        public CodeGenerator Generator { internal get; set; }
        public string Modeler { internal get; set; }
        public string ClientName { internal get; set; }
        public int? PayloadFlattenThreshold { internal get; set; }
        public string HeaderComment { internal get; set; }
        public bool AddCredentials { internal get; set; }
        public string OutputFileName { internal get; set; }
        public bool Verbose { internal get; set; }

        internal void Build(ProcessArgumentBuilder builder)
        {
            if (InputFile == null) throw new ArgumentNullException(nameof(InputFile));
            if (Generator != CodeGenerator.None || GeneratorSettings != null)
            {
                if (GeneratorSettings == null)
                {
                    builder.AppendSwitch(ArgumentNames.Generator, Generator.ToString());
                }
                else
                {
                    builder.AppendSwitch(ArgumentNames.Generator,
                        Generator == CodeGenerator.None
                        ? GeneratorSettings.Generator.ToString()
                        : Generator.ToString());
                    builder.Append(string.Join(" ",
                        GeneratorSettings.GetArguments()
                            .Select(
                                p =>
                                    $"{(p.Key.StartsWith(ArgumentNames.Separator) ? string.Empty : ArgumentNames.Separator)}{p.Key} {p.Value}")));
                }
            }
            if (Namespace.IsNotEmpty()) builder.AppendSwitch(ArgumentNames.Namespace, Namespace.Quote());
            if (OutputDirectory != null) builder.AppendSwitch(ArgumentNames.OutputDir, OutputDirectory.FullPath.Quote());
            if (Modeler.IsNotEmpty()) builder.AppendSwitch(ArgumentNames.Modeler, Modeler);
            if (ClientName.IsNotEmpty()) builder.AppendSwitch(ArgumentNames.ClientName, ClientName);
            if (PayloadFlattenThreshold.HasValue)
            {
                builder.AppendSwitch(ArgumentNames.Threshold, PayloadFlattenThreshold.Value.ToString());
            }
            if (HeaderComment.IsNotEmpty()) builder.AppendSwitch(ArgumentNames.Header, HeaderComment.Quote());
            if (AddCredentials) builder.AppendSwitch(ArgumentNames.Credentials, "true");
            if (OutputFileName.IsNotEmpty()) builder.AppendSwitch(ArgumentNames.OutputFile, OutputFileName.Quote());
            if (Verbose) builder.AppendSwitch(ArgumentNames.Verbose, "true");
            builder.AppendSwitch(ArgumentNames.InputFile, InputFile.FullPath.Quote());
        }
    }

    internal static class CoreExtensions
    {
        
        [DebuggerStepThrough]
        internal static string Argument(this string s)
        {
            return $"{ArgumentNames.Separator}{s}";
        }

        [DebuggerStepThrough]
        internal static bool IsNotEmpty(this string arg)
        {
            return !string.IsNullOrWhiteSpace(arg);
        }

        [DebuggerStepThrough]
        [Obsolete("Just use built-ins you heathen")]
        internal static void AppendArg(this ProcessArgumentBuilder builder, string argName, params string[] values)
        {
            var v = string.Empty;
            if (values.Any()) v = string.Join(" ", values);
            builder.Append($"{ArgumentNames.Separator}{argName} {v}");
        }

        [DebuggerStepThrough]
        internal static void AppendArgQuoted(this ProcessArgumentBuilder builder, string argName, bool quoteValue, params string[] values)
        {
            var v = string.Empty;
            if (values.Any()) v = string.Join(" ", values);
            builder.Append($"{ArgumentNames.Separator}{argName} {v}");
        }
    }

    internal static class ArgumentNames
    {
        internal const string Separator = "-";
        internal const string OutputDir = "-OutputDirectory";
        internal const string OutputFile = "-OutputFileName";
        internal const string Namespace = "-Namespace";
        internal const string Modeler = "-Modeler";
        internal const string ClientName = "-ClientName";
        internal const string Threshold = "-PayloadFlatteningThreshold";
        internal const string Header = "-Header";
        internal const string Credentials = "-AddCredentials";
        internal const string Verbose = "-Verbose";
        internal const string Generator = "-CodeGenerator";
        internal const string InputFile = "-Input";
    }
}