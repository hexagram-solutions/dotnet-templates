# dotnet-templates

Project templates for use with `dotnet new`.

## Installation and Use

Templates can be installed as a nuget package with the command:

```powershell
dotnet new --install Hexagrams.Templates
```

Read more about `dotnet new` and project templates
[here](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-new).

Once installed, the templates can be used just like any other `dotnet new`
template. For example, to create a new *.editorconfig* file, run this command:

```powershell
dotnet new hexagrams-editorconfig
```

Templates can be updated with `dotnet new --update-apply`.

## Adding New Templates

New templates added under the `src/Templates` folder will be included in the
`Hexagrams.Templates` package automatically. For more information on creating
custom templates, see the [official docs](https://docs.microsoft.com/en-us/dotnet/core/tools/custom-templates).

### Solution File

The requirements for [template packages](https://docs.microsoft.com/en-us/dotnet/core/tutorials/cli-templates-create-template-package)
make the solution file in this repo a bit weird. The template package project,
`Hexagrams.Templates` doesn't include any children in compilation, otherwise all
dependencies of templates in the package would have to be included in that
project. `Hexagrams.Templates` only exists as a target for packaging.

To facilitate development and building of these packages in an IDE, the solution
file contains both the package project, and direct references to the template
projects inside the directory structure. The directory structure is the primary
way of structuring the template packages, but template **projects** can be
added to the solution file as needed. Projects under `Hexagrams..Templates` are
**not built** unless included in the solution directly.

In short, if you open the solution in an IDE, you will see template projects in
two places. Once as an item in the `Hexagrams.Templates` project, and once as a
direct inclusion in the solution. This is intended.
