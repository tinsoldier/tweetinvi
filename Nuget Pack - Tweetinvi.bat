REM nuget pack Gnip.Powertrack\Gnip.Powertrack.csproj -IncludeReferencedProjects -properties Configuration=Release -Build
REM nuget push Gnip.Powertrack.1.1.0.0.nupkg QtH9ckxOVN7ylqQzeEn3 -Source http://swannuget.azurewebsites.net/api/v2/package
REM D:\SwanIsland TFS\Tweetinvi\tools\TweetinviAPI\TweetinviAPI.nuspec
REM nuget pack tools\TweetinviAPI\TweetinviAPI.nuspec -IncludeReferencedProjects -properties Configuration=Release -Build
REM cd src/Tweetinvi
REM dotnet pack -c Release
REM nuget push -Source "SwanIslandNuget" -ApiKey VSTS ./src/Tweetinvi/bin/Release/Tweetinvi.5.0.3.nupkg

dotnet pack .\Tweetinvi.NETCore.sln --output nupkgs

dotnet nuget push "nupkgs\Tweetinvi.*.nupkg" --source "SwanIslandNuget" --api-key VSTS --skip-duplicate --interactive