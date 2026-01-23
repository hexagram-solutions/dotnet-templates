using Hexagrams.Nuke.Components;
using Nuke.Common.CI.GitHubActions;

[GitHubActions(
    "continuous",
    GitHubActionsImage.WindowsLatest,
    GitHubActionsImage.UbuntuLatest,
    GitHubActionsImage.MacOsLatest,
    FetchDepth = 0,
    OnPullRequestBranches = ["main"],
    OnPushBranches = ["main", "release/v*"],
    PublishArtifacts = true,
    InvokedTargets = [nameof(Install)],
    CacheKeyFiles = ["global.json", "src/**/*.csproj"])]
[GitHubActions(
    "release",
    GitHubActionsImage.UbuntuLatest,
    FetchDepth = 0,
    OnPushTags = ["v*"],
    PublishArtifacts = true,
    InvokedTargets = [nameof(IPack.Pack), nameof(IPush.Push)],
    CacheKeyFiles = ["global.json", "src/**/*.csproj"],
    ImportSecrets = [nameof(IPush.NuGetApiKey)])]
partial class Build
{
}
