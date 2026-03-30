# CTScope

CTScope is a .NET 8 solution skeleton for a Windows desktop medical CT workflow.

## Repository layout

- `src/CTScope.UI` - WPF desktop UI for folder-based CT study workflow.
- `src/CTScope.Dicom` - DICOM-oriented folder analysis and volume-loading stubs.
- `src/CTScope.Analysis` - Fragment analysis stubs independent from DICOM implementation.
- `tests/CTScope.Tests` - xUnit test project for smoke and future unit tests.
- `docs/architecture.md` - short architecture overview and dependency rules.

This initial iteration intentionally contains only minimal placeholders and no real DICOM parsing.
