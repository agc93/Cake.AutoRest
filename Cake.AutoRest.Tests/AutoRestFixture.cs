using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core.IO;
using Cake.Testing;
using Cake.Testing.Fixtures;

namespace Cake.AutoRest.Tests
{
    public class AutoRestFixture : ToolFixture<AutoRestSettings>
    {
        internal FilePath InputFile => "./swagger.json";
        public AutoRestFixture() : base("AutoRest.exe")
        {
            PrepareData();
        }

        private void PrepareData()
        {
            var json = Properties.Resources.PetStoreExampleJson;
            if (!FileSystem.Exist(InputFile)) FileSystem.CreateFile(InputFile);
            FileSystem.GetFile(InputFile).SetContent(json);
            Settings = new AutoRestSettings(InputFile);
        }

        protected override void RunTool()
        {
            var tool = new AutoRestRunner(FileSystem, Environment, ProcessRunner, Tools);
            ActionSettings?.Invoke(Settings);
            var directoryPath = tool.Generate(InputFile, Settings);
        }

        internal Action<AutoRestSettings> ActionSettings { get; set; }
    }
}
