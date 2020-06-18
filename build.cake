#load "build/helpers.cake"
#load "build/publish.cake"
#tool nuget:?package=DocFx.Console&version=2.55.0
#addin nuget:?package=Cake.DocFx&version=0.13.1

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var framework = Argument("framework", "netstandard2.0");

///////////////////////////////////////////////////////////////////////////////
// VERSIONING
///////////////////////////////////////////////////////////////////////////////

var packageVersion = string.Empty;
#load "build/version.cake"

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////

var solutionPath = File("./src/Cake.AutoRest.sln");
var projects = GetProjects(solutionPath, configuration);
var artifacts = "./dist/";
var testResultsPath = MakeAbsolute(Directory(artifacts + "./test-results"));

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(ctx =>
{
	// Executed BEFORE the first task.
	Information("Running tasks...");
	Information("Running tasks...");
    packageVersion = BuildVersion(fallbackVersion);
	if (FileExists("./build/.dotnet/dotnet.exe")) {
		Information("Using local install of `dotnet` SDK!");
		Context.Tools.RegisterFile("./build/.dotnet/dotnet.exe");
	}
});

Teardown(ctx =>
{
	// Executed AFTER the last task.
	Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASK DEFINITIONS
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
	.Does(() =>
{
	// Clean solution directories.
	foreach(var path in projects.AllProjectPaths)
	{
		Information("Cleaning {0}", path);
		CleanDirectories(path + "/**/bin/" + configuration);
		CleanDirectories(path + "/**/obj/" + configuration);
	}
	Information("Cleaning common files...");
	CleanDirectory(artifacts);
});

Task("Restore")
	.Does(() =>
{
	// Restore all NuGet packages.
	Information("Restoring solution...");
	//NuGetRestore(solutionPath);
	foreach (var project in projects.AllProjectPaths) {
		DotNetCoreRestore(project.FullPath);
	}
});

Task("Build")
	.IsDependentOn("Clean")
	.IsDependentOn("Restore")
	.Does(() =>
{
	Information("Building solution...");
	var settings = new DotNetCoreBuildSettings {
		Configuration = configuration,
		NoIncremental = true,
		ArgumentCustomization = args => args.Append($"/p:Version={packageVersion}")
	};
	DotNetCoreBuild(solutionPath, settings);
});

Task("Run-Unit-Tests")
	.IsDependentOn("Build")
	.Does(() =>
{
	if (projects.TestProjects.Any()) {
		CreateDirectory(testResultsPath);

		var settings = new DotNetCoreTestSettings {
			Configuration = configuration,
			//ArgumentCustomization = args => args.AppendSwitchQuoted("--logger", "trx;LogFilePath=" + testResultsPath + "tests.trx")
		};

		foreach(var project in projects.TestProjects) {
			DotNetCoreTest(project.Path.FullPath, settings);
		}
	}
});

Task("Generate-Docs").Does(() => {
	DocFx("./docfx/docfx.json");
	Zip("./docfx/_site/", artifacts + "/docfx.zip");
});

Task("Post-Build")
	.IsDependentOn("Build")
	.IsDependentOn("Run-Unit-Tests")
	.IsDependentOn("Generate-Docs")
	.Does(() =>
{
	CreateDirectory(artifacts + "build");
	foreach (var project in projects.SourceProjects) {
		var buildDir = artifacts + "build/" + project.Name;
		CreateDirectory(buildDir);
		var files = GetFiles(project.Path.GetDirectory() + "/bin/" + configuration + "/**/" + project.Name +".*");
		CopyFiles(files, buildDir);
	}
});

Task("Pack")
	.IsDependentOn("Post-Build")
	.Does(() =>
{
	Information("Building NuGet packages");
	var nupkgDir = $"{artifacts}/nuget/";
	CreateDirectory(nupkgDir);
	var versionNotes = ParseAllReleaseNotes("./ReleaseNotes.md").FirstOrDefault(v => v.Version.ToString() == packageVersion);
	var releaseNotes = versionNotes != null ? string.Join(Environment.NewLine, versionNotes.Notes.ToList()) : "";
	var packSettings = new DotNetCorePackSettings
	{
		Configuration = configuration,
		OutputDirectory = nupkgDir,
		IncludeSymbols = true,
		NoBuild = true,
		ArgumentCustomization = args => args.Append($"/p:PackageVersion={packageVersion}").Append($"/p:PackageReleaseNotes=\"{releaseNotes}\"")
	};
	DotNetCorePack(solutionPath, packSettings);
});

Task("Default")
	.IsDependentOn("Pack");

RunTarget(target);
