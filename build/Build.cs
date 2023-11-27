using System.Collections.Generic;
using System.Linq;
using Hexagrams.Nuke.Components;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.ControlFlow;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

// ReSharper disable RedundantExtendsListEntry
// ReSharper disable InconsistentNaming

[DotNetVerbosityMapping]
[ShutdownDotNetAfterServerBuild]
partial class Build : NukeBuild,
    IHasGitRepository,
    IHasVersioning,
    IClean,
    IRestore,
    IFormat,
    ICompile,
    IPack,
    IPush
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main() => Execute<Build>(x => ((ICompile) x).Compile);

    [Solution]
    readonly Solution Solution;
    Solution IHasSolution.Solution => Solution;

    public IEnumerable<AbsolutePath> ExcludedFormatPaths => Enumerable.Empty<AbsolutePath>();

    public bool RunFormatAnalyzers => true;

    Target ICompile.Compile => t => t
        .Inherit<ICompile>()
        .DependsOn<IFormat>(x => x.VerifyFormat);

    Configure<DotNetPublishSettings> ICompile.PublishSettings => t => t
        .When(!ScheduledTargets.Contains(((IPush) this).Push), s => s
            .ClearProperties());

    Project TemplatesProject => Solution.GetAllProjects("Hexagrams.Templates").Single();

    Configure<DotNetPackSettings> IPack.PackSettings => t => t
        .SetProject(TemplatesProject);

    Target IPush.Push => t => t
        .Inherit<IPush>()
        .Consumes(this.FromComponent<IPack>().Pack)
        .Requires(() => this.FromComponent<IHasGitRepository>().GitRepository.Tags.Any())
        .WhenSkipped(DependencyBehavior.Execute);

    Target Install => t => t
        .Description("Tests template package installation by building and re-installing the package locally.")
        .DependsOn<IPack>()
        .Executes(() =>
        {
            var packageName = TemplatesProject.Name;
            var version = this.FromComponent<IHasVersioning>().Versioning.NuGetVersionV2;
            var packagePath = this.FromComponent<IPack>().PackagesDirectory / $"{packageName}.{version}.nupkg";

            SuppressErrors(() => DotNet($"new uninstall {packageName}"));
            DotNet($"new install {packagePath}");
        });
}
