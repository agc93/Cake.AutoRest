# Packages

You can include the addin in your script with:

```
#addin nuget:?package=Cake.AutoRest
//or to use the latest development release
#addin nuget:?package=Cake.AutoRest&prerelease

// you will also need AutoRest itself
#tool "AutoRest"
```

The NuGet prerelease packages are automatically built and deployed from the `develop` branch so they can be considered bleeding-edge while the non-prerelease packages will be much more stable.

Versioning is predominantly SemVer-compliant so you can set your version constraints if you're worried about changes.

> **NOTE:** These packages **DO NOT** include AutoRest, so you will need to include it as above, as Cake is currently unable to correctly reference NuGet dependencies.