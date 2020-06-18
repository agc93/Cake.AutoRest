#tool "nuget:https://api.nuget.org/v3/index.json?package=nuget.commandline&version=5.5.1"

Task("Publish-NuGet-Packages")
.IsDependentOn("Pack")
.WithCriteria(() => !string.IsNullOrWhiteSpace(EnvironmentVariable("NUGET_TOKEN")))
.WithCriteria(() => EnvironmentVariable("GITHUB_REF").StartsWith("refs/tags/v"))
.Does(() => {
    var nupkgDir = $"{artifacts}nuget";
    var nugetToken = EnvironmentVariable("NUGET_TOKEN");
    var pkgFiles = GetFiles($"{nupkgDir}/*.nupkg");
    NuGetPush(pkgFiles, new NuGetPushSettings {
      Source = "https://api.nuget.org/v3/index.json",
      ApiKey = nugetToken
    });
});

Task("Release")
.IsDependentOn("Publish-NuGet-Packages");