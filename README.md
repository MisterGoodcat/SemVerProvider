SemVerProvider
==============

A C# library that provides sematic version numbers (see http://semver.org/), for use as-is, and wrapped as a web service.

## Features

__VersionProvider.Core__

* Generates correct semantic version numbers by increasing major, minor or patch level
* Supports pre-release identifiers and build metadata extensions
* Validates versions against the definitions at semver.org
* Default persistence uses file system, handles concurrency issues properly
* Can be used and extended in any other .NET project
* Fully unit and integration tested (see VersionProvider.Core.Tests and VersionProvider.Core.IntegrationTests)

__SemVerServer__

* A Web API sample project that exposes VersionProvider.Core features as web service
* Supports multiple version "scopes" (i.e. components, projects etc. that you want to version) through configuration

## Samples

For core usage samples, see the unit and integration test projects.

Examples of talking to the sample web service in SemVerServer:

    GET /Api/SemVer/GetCurrentVersion/MyProject

Initializes with and returns 0.1.0 for the first call, or the current version, respectively

    GET /Api/SemVer/GetNextBugFixVersion/MyProject

Increases the patch level, i.e. 0.1.0 => 0.1.1

    GET /Api/SemVer/GetNextFeatureVersion/MyProject

Increases the minor version, resets the patch level, i.e. 0.1.1 => 0.2.0

    GET /Api/SemVer/GetNextPreRelaseVersion/MyProject?preReleaseIdentifier=alpha.1

Sets the prerelease extensions without touching the major, minor, patch levels, i.e. 0.2.0-alpha.1

    GET /Api/SemVer/GetNextBugFixVersion/MyProject?preReleaseIdentifier=rc-1

Gets the next bug fix version and sets the prerelease identifier, i.e. 0.2.1-rc-1

    GET /Api/SemVer/GetNextBugFixVersion/MyProject?preReleaseIdentifier=rc.02

Returns error 400 Bad Request with an appropriate validation error message because the numeric parts of a pre-release identifier must not contain leading zeros

    POST /Api/SemVer/SetVersion/MyProject

Hard-(re-)sets the version to the given SemanticVersionInfo struct contained in the request body

All response formats are either XML or JSON encoded, depending on the request headers, and look like:

    {"Major":0,"Minor":1,"Patch":1,"PreReleaseIdentifier":null,"BuildMetadata":null,"FormattedVersion":"0.1.1"}

__Have fun!__

