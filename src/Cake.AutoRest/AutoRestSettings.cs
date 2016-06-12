using System;
using System.Diagnostics;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.AutoRest
{
    /// <summary>
    /// Settings to control the AutoRest generation process.
    /// </summary>
    /// <remarks>Supports either standard object syntax or a fluent interface using extension methods.</remarks>
    public class AutoRestSettings : ToolSettings
    {
        /// <summary>
        /// Create a new instances of the <see cref="AutoRestSettings"/> class.
        /// </summary>
        /// <param name="inputFile">File containing a compatible API specification</param>
        public AutoRestSettings(FilePath inputFile)
        {
            InputFile = inputFile;
        }

        /// <summary>
        /// Create a new instances of the <see cref="AutoRestSettings"/> class.
        /// </summary>
        public AutoRestSettings()
        {
            
        }

        internal FilePath InputFile { get; set; }

        /// <summary>
        /// Any generator-specific settings.
        /// </summary>
        /// <remarks>Currently only implemented by CSharpGeneratorSettings</remarks>
        public IGeneratorSettings GeneratorSettings { get; set; }
        /// <summary>
        /// The namespace to use for generated code
        /// </summary>
        public string Namespace { internal get; set ; }
        /// <summary>
        /// The location for generated files.
        /// </summary>
        /// <value>
        /// Directory path for generated code output. If not specified, uses "Generated" as the default
        /// </value>
        public DirectoryPath OutputDirectory { internal get; set; }
        /// <summary>
        /// The code generator (see <see cref="CodeGenerator"/>) language.
        /// </summary>
        /// <value>
        /// If not specified, defaults to CSharp.
        /// </value>
        public CodeGenerator Generator { internal get; set; }
        /// <summary>
        /// The Modeler to use on the input.
        /// </summary>
        /// <value>If not specified, defaults to Swagger.</value>
        public string Modeler { internal get; set; }
        /// <summary>
        /// Name to use for the generated client type. 
        /// </summary>
        /// <value>By default, uses the value of the 'Title' field from the Swagger input.</value>
        public string ClientName { internal get; set; }
        /// <summary>
        /// The maximum number of properties in the request body to be represented as method arguments.
        /// </summary>
        /// <value>
        /// If the number of properties in the request body is less than or equal to this value, these properties will be represented as method arguments
        /// </value>
        public int? PayloadFlattenThreshold { internal get; set; }
        /// <summary>
        /// Text to include as a header comment in generated files.
        /// </summary>
        /// <value>Use NONE to suppress the default header.</value>
        public string HeaderComment { internal get; set; }
        /// <summary>
        /// If true, the generated client includes a ServiceClientCredentials property and constructor parameter. Authentication behaviors are implemented by extending the ServiceClientCredentials type
        /// </summary>
        public bool AddCredentials { internal get; set; }
        /// <summary>
        /// If set, will cause generated code to be output to a single file.
        /// </summary>
        /// <remarks>
        /// Not supported by all code generators.
        /// </remarks>
        public string OutputFileName { internal get; set; }
        /// <summary>
        /// If set, will output verbose diagnostic messages.
        /// </summary>
        public bool Verbose { internal get; set; }

        internal void Build(ProcessArgumentBuilder builder)
        {
            if (InputFile == null) throw new ArgumentNullException(nameof(InputFile));
            if (Generator != CodeGenerator.None || GeneratorSettings != null)
            {
                if (GeneratorSettings == null)
                {
                    builder.AppendSwitch(ArgumentNames.Generator, Generator.ToGeneratorName());
                }
                else
                {
                    builder.AppendSwitch(ArgumentNames.Generator,
                        Generator == CodeGenerator.None
                        ? GeneratorSettings.Generator.ToGeneratorName()
                        : Generator.ToGeneratorName());
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
        internal static bool IsNotEmpty(this string arg)
        {
            return !string.IsNullOrWhiteSpace(arg);
        }

        /// <summary>
        /// Wraps ToString() with additional special processing for Azure generators
        /// </summary>
        /// <param name="gen">The generator to get a name of</param>
        /// <returns>CLI-friendly name of the generator</returns>
        /// <remarks>This could probably be replaced by using the <see cref="System.ComponentModel.DescriptionAttribute"/> class</remarks>
        [DebuggerStepThrough]
        internal static string ToGeneratorName(this CodeGenerator gen)
        {
            var s = gen.ToString();
            if (s.StartsWith("Azure")) s = s.Replace("Azure", "Azure.");
            return s;
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