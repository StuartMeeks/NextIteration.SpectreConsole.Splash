# Changelog

All notable changes to `NextIteration.SpectreConsole.Splash` are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [Unreleased]

### Changed

- **Test suite migrated to xUnit.net v3 (`xunit.v3` 4.0.0)** from `xunit` 2.9.3.
  Contributor-facing only — no library code, public API, or shipped package
  contents changed. v3 test projects are self-executing console apps and run on
  [Microsoft.Testing.Platform](https://learn.microsoft.com/dotnet/core/testing/microsoft-testing-platform-intro)
  rather than VSTest, so a root `global.json` now opts `dotnet test` into the
  MTP runner. `Microsoft.NET.Test.Sdk` and `xunit.runner.visualstudio` are no
  longer needed and were dropped.

- **Tests now run against both shipped TFMs.** The test project multi-targets
  `net8.0;net10.0`, mirroring the library, so `lib/net8.0` is actually executed
  against its own dependency graph instead of only being compiled. Previously
  the suite ran on `net10.0` only. Running the full suite locally now requires
  the .NET 8 runtime alongside the .NET 10 SDK.

### Removed

- **`Microsoft.SourceLink.GitHub` package reference.** The .NET SDK has bundled
  SourceLink since .NET 8; the explicit reference was redundant. Verified
  byte-identical `.nuspec` and `.snupkg` output either way — consumers still get
  repository metadata and step-through sources.

### Fixed

- **`PackageOutputPath` no longer breaks non-Windows builds.** The hardcoded
  `C:\nuget-local\` local dev feed is now guarded to Windows; previously every
  `dotnet build` on Linux/macOS and in CI created a literal `C:\nuget-local\`
  directory under `src/`.
- **CI publish job could not check out the repo.** Its `permissions` block listed
  only `id-token: write`, and GitHub sets every unlisted scope to `none`, leaving
  `actions/checkout` without `contents: read`.

---

## [0.3.0] — 2026-07-24

### Changed

- **Dependency floor raised to Spectre.Console `0.57.2`** (from `0.56.0`). The
  package is now built and tested against `0.57.2`, so that becomes the minimum
  version NuGet resolves for consumers. No public API changes.

---

## [0.2.0] — 2026-06-20

### Added

- **Multi-targeting `net8.0` and `net10.0`.** The package now ships `lib/net8.0`
  and `lib/net10.0` assets, so consumers on .NET 8 can use it without forcing an
  upgrade to .NET 10. No public API changes.

---

## [0.1.2] — 2026-05-03

### Changed

- **Symbol packaging.** Switched `<DebugType>` from `embedded` to `portable`
  so the published `.snupkg` actually contains `.pdb` files. The previous
  combination produced an empty `.snupkg` — fine for the workflow (which
  uploaded only `*.nupkg` as an artifact, silently dropping the symbol
  package), but it meant no symbols ever reached nuget.org's symbol
  server. Consumers debugging into the library now get sources via the
  symbol server out of the box.
- **CI artifact path.** `upload-artifact` now captures `*nupkg` (both
  `.nupkg` and `.snupkg`) so the publish job pushes both files.

---

## [0.1.1] — 2026-04-18

### Changed

- **Now published to NuGet** — consume via
  `dotnet add package NextIteration.SpectreConsole.Splash`. Brings the
  package in line with the rest of the `NextIteration.SpectreConsole.*`
  family, which are all on nuget.org. No API changes from 0.1.0.
- Package metadata added (description, tags, project URL, license
  expression, readme, icon). SourceLink enabled with embedded PDBs —
  step-through debugging works out of the box for consumers.

---

## [0.1.0] — 2026-04-18

### Added — initial public release

- **`SplashScreen.Show(string)`** — one-call splash for a given CLI name using
  all defaults (Roman font, neutral blue gradient, random built-in tagline).
- **`SplashScreen.Show(SplashOptions)`** — full configurability: custom
  Figgle font, gradient stops, and tagline strategy.
- **`SplashColors`** — gradient palette with piecewise-linear interpolation
  across N stops. Validates `#RRGGBB` format at construction. No upper
  bound on stop count (compute cost is O(logo-width) regardless).
- **`SplashTagline`** — three strategies:
  - `None` — no tagline
  - `RandomBuiltIn` — pick from ~300 built-in developer-culture quotes
  - `FromProvider(Func<string?>)` — bring your own
- **Performance**: the entire splash — logo + tagline — is assembled as a
  single Spectre.Console markup string and emitted via one
  `AnsiConsole.Markup` call. The previous per-character write pattern was
  dominated by ANSI-escape flushing on a per-char basis; batching cut
  cold-start render time from ~60 ms to <10 ms on a typical 64-column
  Roman logo.
- Demo project and test suite (40 xUnit tests covering gradient
  interpolation, colour validation, tagline strategies, renderer output,
  and the quote pool).

[Unreleased]: https://github.com/StuartMeeks/NextIteration.SpectreConsole.Splash/compare/v0.3.0...HEAD
[0.3.0]: https://github.com/StuartMeeks/NextIteration.SpectreConsole.Splash/releases/tag/v0.3.0
[0.2.0]: https://github.com/StuartMeeks/NextIteration.SpectreConsole.Splash/releases/tag/v0.2.0
[0.1.2]: https://github.com/StuartMeeks/NextIteration.SpectreConsole.Splash/releases/tag/v0.1.2
[0.1.1]: https://github.com/StuartMeeks/NextIteration.SpectreConsole.Splash/releases/tag/v0.1.1
[0.1.0]: https://github.com/StuartMeeks/NextIteration.SpectreConsole.Splash/releases/tag/v0.1.0
