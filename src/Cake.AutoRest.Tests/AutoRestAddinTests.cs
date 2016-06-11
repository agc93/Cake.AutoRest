using System;
using System.Linq;
using Cake.Core;
using FluentAssertions;
using Shouldly;
using Xunit;

namespace Cake.AutoRest.Tests
{
    public class AutoRestAddinTests : AutoRestTestBase
    {
        [Fact]
        public void InputPathSetCorrectly()
        {
            var result = Fixture.Run();
            result.Args.Should().Contain("-Input").And.Contain(Fixture.InputFile.FullPath);
        }

        [Fact]
        public void OnlyInputPathSetForDefaults()
        {
            var result = Fixture.Run();
            result.Args.Split(' ').Count(a => a.StartsWith("-")).ShouldBe(1);
        }

        [Fact]
        public void ShouldIncludeNamespaceWhenSet()
        {
            Fixture.Settings = new AutoRestSettings(Fixture.InputFile)
            {
                Namespace = "Cake.AutoRest"
            };
            var result = Fixture.Run();
            result.Args.Should().Contain("-Namespace").And.Contain("Cake.AutoRest");
        }

        [Fact]
        public void ShouldIncludeOutputDirectoryWhenSet()
        {
            Fixture.Settings = new AutoRestSettings(Fixture.InputFile)
            {
                OutputDirectory = "./publish"
            };
            var result = Fixture.Run();
            result.Args.Should()
                .Contain("-OutputDirectory", "the output dir has been set")
                .And.Contain("\"publish\"", "Cake should have quoted the path");
        }

        [Fact]
        public void ShouldIncludeGeneratorWhenSet()
        {
            Fixture.Settings = new AutoRestSettings(Fixture.InputFile)
            {
                Generator = CodeGenerator.CSharp
            };
            var result = Fixture.Run();
            result.Args.Should().Contain("-CodeGenerator").And.Contain(" CSharp ");
        }

        [Fact]
        public void ShouldIncludeModelerWhenSet()
        {
            Fixture.Settings = new AutoRestSettings(Fixture.InputFile)
            {
                Modeler = "swagger"
            };
            var result = Fixture.Run();
            result.Args.Should().Contain("-Modeler").And.Contain(" swagger ");
        }

        [Fact]
        public void ShouldIncludeClientNameWhenSet()
        {
            Fixture.ActionSettings = s =>
            {
                s.ClientName = "Cake.AutoRest";
            };
            var result = Fixture.Run();
            result.Args.Should().Contain("-ClientName").And.Contain("Cake.AutoRest");
        }

        [Fact]
        public void ShouldIncludeFlattenWhenSet()
        {
            Fixture.ActionSettings = s => s.PayloadFlattenThreshold = 5;
            var result = Fixture.Run();
            result.Args.Should().Contain("-PayloadFlatteningThreshold").And.Contain("5");
        }

        [Fact]
        public void ShouldIncludeQuotedCommentWhenSet()
        {
            Fixture.ActionSettings = s => s.HeaderComment = "Header Comment";
            var result = Fixture.Run();
            result.Args.Should().Contain("-Header").And.Contain("\"Header Comment\"");
        }

        [Fact]
        public void ShouldIncludeCredentialSwitchWhenSet()
        {
            Fixture.ActionSettings = s => s.AddCredentials = true;
            var result = Fixture.Run();
            result.Args.Should().Contain("-AddCredentials true", "this switch doesn't support values");
        }

        [Fact]
        public void ShouldIncludeOutputFileWhenSet()
        {
            Fixture.ActionSettings = s => s.OutputFileName = "client.cs";
            var result = Fixture.Run();
            result.Args.Should().Contain("-OutputFileName").And.Contain("\"client.cs\"");
        }

        [Fact]
        public void ShouldIncludeVerboseWhenSet()
        {
            Fixture.ActionSettings = s => s.Verbose = true;
            var result = Fixture.Run();
            result.Args.Should().Contain("-Verbose true", "this switch doesn't support values");
        }

        [Fact]
        public void ShouldIncludeAllGeneratorSettings()
        {
            Fixture.ActionSettings = s => s.GeneratorSettings = new CSharpGeneratorSettings()
            {
                InternalConstructors = true,
                SyncMethodMode = "Essential",
                UseDateTimeOffset = true
            };
            var result = Fixture.Run();
            result.Args.Should()
                .Contain("-CodeGenerator CSharp", "this switch should be provided by gen settings")
                .And.Contain("-SyncMethods Essential")
                .And.Contain("-InternalConstructors true")
                .And.Contain("-UseDateTimeOffset true");
        }

        [Fact]
        public void ShouldRespectExplicitGeneratorSettings()
        {
            Fixture.ActionSettings = s =>
            {
                s.GeneratorSettings = new CSharpGeneratorSettings
                {
                    InternalConstructors = true,
                };
                s.Generator = CodeGenerator.NodeJS;
            };
            var result = Fixture.Run();
            result.Args.Should()
                .Contain("-CodeGenerator NodeJS", "explicit generators override settings-provided ones")
                .And.Contain("-InternalConstructors true", "settings still show up regardless");
        }

        [Fact]
        public void ShouldHandleAzureGeneratorNames()
        {
            Fixture.ActionSettings = s =>
            {
                s.Generator = CodeGenerator.AzureCSharp;
            };
            var result = Fixture.Run();
            result.Args
                .Should()
                .Contain("-CodeGenerator Azure.CSharp", "Azure-specific gens use a dot in the name");
        }
    }
}
