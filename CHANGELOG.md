# Changelog

All notable changes to `NextIteration.SpectreConsole.Splash` are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

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

### Not published to NuGet

This package is consumed via git submodule + `<ProjectReference>`. See the
README for setup instructions.

[0.1.0]: https://github.com/StuartMeeks/NextIteration.SpectreConsole.Splash/releases/tag/v0.1.0
