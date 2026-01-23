# CLAUDE.md

This file provides guidance to [Claude Code](https://claude.ai/code) when
working with code in this repository.

## Project Overview

This repository contains `dotnet new` project templates packaged as a NuGet
package (`Hexagrams.Templates`). Templates are installed via
`dotnet new install Hexagrams.Templates` and used with commands like
`dotnet new hexagrams-editorconfig`.

## Build System

This project uses [NUKE](https://nuke.build/) for builds. The build project is
in the `build/` directory.

### Common Commands

```powershell
# Using NUKE global tool (recommended for tab completion)
nuke               # Run default build
nuke test          # Run unit tests
nuke compile       # Build the solution
nuke format        # Format code
nuke verify-format # Verify code formatting
nuke pack          # Pack the template package
```

## Architecture

### Template Package Structure

- `src/Hexagrams.Templates.csproj` - The NuGet package project. This is a
  `PackageType=Template` project that packages everything under `src/Templates/`
  but does not compile template code.
- `src/Templates/` - Contains the actual templates. Each subdirectory is a
  template with a `.template.config/template.json` manifest.

### Available Templates

| Short Name                | Description                                  |
| ------------------------- | -------------------------------------------- |
| `hexagrams-editorconfig`  | .editorconfig with C# code style preferences |
| `hexagrams-gitattributes` | .gitattributes for .NET repositories         |
| `hexagrams-gitversion`    | GitVersion configuration for release flow    |
| `hexagrams-test`          | xUnit test project with FluentAssertions     |

### Solution File Note

Template projects appear twice in the solution: once as items within
`Hexagrams.Templates` (for packaging) and once as direct project references (for
IDE development/building). This is intentional per the README.

### Build Targets (NUKE)

The build inherits from `Hexagrams.Nuke.Components` providing these interfaces:

- `IClean`, `IRestore`, `IFormat`, `ICompile`, `IPack`, `IPush`
- `IHasGitRepository`, `IHasVersioning` (uses GitVersion for semantic
  versioning)

The `Install` target is custom - it packs the templates and reinstalls them
locally for testing.

## Adding New Templates

1. Create a new directory under `src/Templates/`
2. Add a `.template.config/template.json` manifest defining the template
   metadata
3. The template will be automatically included in the package (no csproj changes
   needed)

See
[Microsoft's template docs](https://docs.microsoft.com/en-us/dotnet/core/tools/custom-templates)
for template.json schema.
