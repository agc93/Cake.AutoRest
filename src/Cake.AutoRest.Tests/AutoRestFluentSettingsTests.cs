using FluentAssertions;
using Shouldly;
using Xunit;

namespace Cake.AutoRest.Tests
{
    public class AutoRestFluentSettingsTests : AutoRestTestBase
    {

        [Fact]
        public void ShouldSetNamespace()
        {
            Fixture.Settings.UseNamespace("Cake.AutoRest.Tests");
            Fixture.Settings.Namespace.ShouldBe("Cake.AutoRest.Tests");
        }

        [Fact]
        public void ShouldSetOutputDir()
        {
            Fixture.Settings.OutputToDirectory("./publish");
            Fixture.Settings.OutputDirectory.FullPath.ShouldBe("publish");
        }

        [Fact]
        public void ShouldSetGenerator()
        {
            Fixture.Settings.WithGenerator(CodeGenerator.CSharp);
            Fixture.Settings.Generator.ShouldBe(CodeGenerator.CSharp);
        }

        [Fact]
        public void ShouldSetModeler()
        {
            Fixture.Settings.WithModeler("swagger");
            Fixture.Settings.Modeler.ShouldBe("swagger");
        }

        [Fact]
        public void ShouldSetClientName()
        {
            Fixture.Settings.UseClientName("Cake.AutoRest");
            Fixture.Settings.ClientName.ShouldBe("Cake.AutoRest");
        }

        [Fact]
        public void ShouldSetThreshold()
        {
            Fixture.Settings.UseFlattenThreshold(2);
            Fixture.Settings.PayloadFlattenThreshold.ShouldBe(2);
        }

        [Fact]
        public void ShouldSetComment()
        {
            Fixture.Settings.AddHeaderComment("// comment with delimiters  ");
            Fixture.Settings.HeaderComment.ShouldBe("comment with delimiters");
        }

        [Fact]
        public void ShouldSetCredentialsFlag()
        {
            Fixture.Settings.AddCredentials();
            Fixture.Settings.AddCredentials.ShouldBeTrue();
        }

        [Fact]
        public void ShouldSetOutputFileName()
        {
            Fixture.Settings.OutputToFile("./client.cs");
            Fixture.Settings.OutputFileName = "./client.cs";
        }

        [Fact]
        public void ShouldSetVerbosity()
        {
            Fixture.Settings.WithVerboseOutput();
            Fixture.Settings.Verbose.ShouldBeTrue();
        }
    }
}
