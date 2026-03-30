# CTScope Architecture (Iteration 1)

The solution is organized as a single repository with one .NET 8 solution file (`CTScope.sln`) and four projects.

## Projects

- `CTScope.UI` (WPF): presentation layer for a folder-based CT study workflow.
- `CTScope.Dicom` (class library): stubs for DICOM folder analysis and volume loading.
- `CTScope.Analysis` (class library): stubs for fragment analysis processing.
- `CTScope.Tests` (xUnit): smoke test setup and future unit tests.

## Dependency rules

- `CTScope.UI` references `CTScope.Dicom` and `CTScope.Analysis`.
- `CTScope.Dicom` does **not** reference `CTScope.Analysis`.
- `CTScope.Analysis` does **not** reference `CTScope.Dicom`.
- `CTScope.Tests` references `CTScope.Dicom` and `CTScope.Analysis`.

## Current scope

This iteration provides a clean skeleton and placeholder methods only. The workflow is folder-based; single-file opening is out of scope.
