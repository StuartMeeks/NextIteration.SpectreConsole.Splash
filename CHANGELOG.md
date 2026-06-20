# Changelog

All notable changes to `NextIteration.SpectreConsole.Splash` are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

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

[0.1.2]: https://github.com/StuartMeeks/NextIteration.SpectreConsole.Splash/releases/tag/v0.1.2
[0.1.1]: https://github.com/StuartMeeks/NextIteration.SpectreConsole.Splash/releases/tag/v0.1.1
[0.1.0]: https://github.com/StuartMeeks/NextIteration.SpectreConsole.Splash/releases/tag/v0.1.0
