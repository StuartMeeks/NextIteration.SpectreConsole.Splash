# Changelog

All notable changes to `NextIteration.SpectreConsole.Splash` are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [Unreleased]

### Added

- **`Microsoft.SourceLink.GitHub` package reference restored** (§1.7), at the
  estate-wide version 10.0.400 and with `PrivateAssets="All"` so it is never a
  consumer dependency. It had been removed as redundant — the .NET 8+ SDK does emit
  repository metadata and a source-link document map without it, which is accurate as
  far as it goes — but the standard requires the explicit reference so that source
  linking is pinned to a version the estate controls rather than moving with whichever
  SDK feature band happens to build. The `.nuspec`, README, icon and XML docs are
  unaffected; the assembly and symbols shift slightly (see below).
- **Canonical CI shape, CodeQL and Dependabot** per
  [NextIteration.Standards](https://github.com/StuartMeeks/NextIteration.Standards)
  `STANDARD.md` section 3 and 4. `ci.yml` now splits into `build`, a three-platform
  `test` matrix, an aggregating `ci` gate and a tag-gated `publish`; the gate is the
  single required status check, so the matrix can be reshaped without touching branch
  protection. Adds `codeql.yml` (§4.4), `dependabot.yml` (§4.6) and
  `dependabot-auto-merge.yml` (§4.7), plus per-workflow `concurrency`,
  `timeout-minutes` and a NuGet restore cache (§3.5–3.7).
- **Code coverage is collected** via `Microsoft.Testing.Extensions.CodeCoverage`, the
  Microsoft.Testing.Platform equivalent of coverlet (§2.6). CI invokes it and uploads
  the result per platform. Contributor-facing only; the collector is test-only and does
  not reach the package.

### Changed

- **Central Package Management completed and shared build properties centralised.**
  `Directory.Packages.props` gains `CentralPackageVersionOverrideEnabled=false`, so a
  stray inline `Version=` alongside CPM is now a hard build failure instead of being
  silently ignored (§1.3). The seventeen properties every project restated now live in
  the root `Directory.Build.props` (§1.2); each csproj keeps only what is genuinely its
  own. Non-shipping projects (tests, demo) set `GenerateDocumentationFile=false` to opt
  back out. Verified behaviour-preserving: packed before and after from a clean `obj/`
  at the same commit, every entry in both the `.nupkg` and `.snupkg` is byte-identical
  bar the per-pack `.psmdcp` name.
- **Adopted the standards baseline docs, editor config and SDK pin.**
  `SECURITY.md`, `CONTRIBUTING.md`, `CLAUDE.md` and a pull request template added;
  `.gitignore` and `.editorconfig` replaced with the canonical copies; `global.json`
  now pins the SDK band (`10.0.100`, `rollForward: latestFeature`) alongside the
  existing Microsoft.Testing.Platform runner setting. An unpinned SDK gives a
  contributor different analyzer results from CI, and `TreatWarningsAsErrors` turns
  that into a build that fails for them and passes for everyone else.
- **README now states its target frameworks and dependency floors** (§5.3), and
  explains why the floors are not per-target-framework.
- **`SplashTagline.RandomBuiltIn`'s XML doc corrected** from "~200 quotes" to
  "~300": the built-in pool holds 313 entries. Documentation only; the pool itself
  is unchanged.
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

### Fixed

- **`PackageOutputPath` no longer points at a machine-local path.** Pack output
  goes to `artifacts/packages` on every platform (§1.8). It previously pointed at
  a hardcoded `C:\nuget-local\` dev feed; unguarded that created a literal
  `C:\nuget-local\` directory under `src/` on Linux, macOS and in CI, and even
  guarded to Windows it made one contributor's machine layout part of repo config.
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
