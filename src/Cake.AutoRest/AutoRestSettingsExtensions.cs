using Cake.Core.IO;

namespace Cake.AutoRest
{
    /// <summary>
    /// Extension methods to enable fluent configuration of <see cref="AutoRestSettings"/>
    /// </summary>
    public static class AutoRestSettingsExtensions
    {
        /// <summary>
        /// Sets the client namespace
        /// </summary>
        /// <param name="settings">The settings</param>
        /// <param name="ns">Desired client namespace</param>
        /// <returns>The updated settings object</returns>
        public static AutoRestSettings UseNamespace(this AutoRestSettings settings, string ns)
        {
            settings.Namespace = ns;
            return settings;
        }

        /// <summary>
        /// Sets the output directory for generated code
        /// </summary>
        /// <param name="settings">Settings</param>
        /// <param name="path">Directory to output generated code</param>
        /// <returns>The updated settings object</returns>
        public static AutoRestSettings OutputToDirectory(this AutoRestSettings settings, DirectoryPath path)
        {
            settings.OutputDirectory = path.FullPath;
            return settings;
        }

        /// <summary>
        /// Sets the Generator used by AutoRest
        /// </summary>
        /// <param name="settings">Settings</param>
        /// <param name="gen">Desired code generator (any <see cref="CodeGenerator"/> member)</param>
        /// <param name="genSettings">Optional generator-specific settings to use</param>
        /// <returns>The updated settings object</returns>
        public static AutoRestSettings WithGenerator(this AutoRestSettings settings, CodeGenerator gen, IGeneratorSettings genSettings = null)
        {
            settings.Generator = gen;
            if (genSettings != null) settings.GeneratorSettings = genSettings;
            return settings;
        }

        /// <summary>
        /// Sets the modeler used by AutoRest
        /// </summary>
        /// <param name="settings">Settings</param>
        /// <param name="modeler">Desired code modeler name (defaults to 'swagger')</param>
        /// <returns>The updated settings object</returns>
        public static AutoRestSettings WithModeler(this AutoRestSettings settings, string modeler)
        {
            settings.Modeler = modeler;
            return settings;
        }

        /// <summary>
        /// Sets the client name to use in generated code
        /// </summary>
        /// <param name="settings">Settings</param>
        /// <param name="clientName">Client name for generated client</param>
        /// <returns>The updated settings object</returns>
        public static AutoRestSettings UseClientName(this AutoRestSettings settings, string clientName)
        {
            settings.ClientName = clientName;
            return settings;
        }

        /// <summary>
        /// Sets the property flattening threshold (see <see cref="AutoRestSettings"/>)
        /// </summary>
        /// <param name="settings">Settings</param>
        /// <param name="threshold">Maximum number of properties to use method overloading for</param>
        /// <returns>The updated settings object</returns>
        public static AutoRestSettings UseFlattenThreshold(this AutoRestSettings settings, int threshold)
        {
            settings.PayloadFlattenThreshold = threshold;
            return settings;
        }

        /// <summary>
        /// Sets the generated code header comment
        /// </summary>
        /// <param name="settings">Settings</param>
        /// <param name="comment">Header comment to put in generated code. Use 'NONE' to suppress</param>
        /// <returns>The updated settings object</returns>
        public static AutoRestSettings AddHeaderComment(this AutoRestSettings settings, string comment)
        {
            settings.HeaderComment = comment.Trim('/', '#', '*').Trim(); //just remove common comment delimiters
            return settings;
        }

        /// <summary>
        /// Enables the inclusion of a ServiceClientCredentials property and constructor parameter in generated code
        /// </summary>
        /// <param name="settings">Settings</param>
        /// <param name="useCredentials">Set to true to enable credentials (defaults to true)</param>
        /// <returns></returns>
        public static AutoRestSettings AddCredentials(this AutoRestSettings settings, bool useCredentials = true)
        {
            settings.AddCredentials = useCredentials;
            return settings;
        }

        /// <summary>
        /// Will cause generated code to be output to the specified single file. Not supported by all code generators.
        /// </summary>
        /// <param name="settings">Settings</param>
        /// <param name="path">Path to create the single-file client</param>
        /// <returns>The updated settings object</returns>
        public static AutoRestSettings OutputToFile(this AutoRestSettings settings, FilePath path)
        {
            settings.OutputFileName = path.FullPath;
            return settings;
        }

        /// <summary>
        /// Enables verbose/diagnostic messages
        /// </summary>
        /// <param name="settings">Settings</param>
        /// <returns>The updated settings object</returns>
        public static AutoRestSettings WithVerboseOutput(this AutoRestSettings settings)
        {
            settings.Verbose = true;
            return settings;
        }

    }
}
