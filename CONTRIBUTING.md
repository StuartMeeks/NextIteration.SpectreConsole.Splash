# Contributing

Issues and pull requests are welcome.

## Before you open a PR

- **The build must be clean.** `TreatWarningsAsErrors` is on and analyzers run at
  `latest`. A warning is a build failure, not a suggestion.
- **Tests run on every target framework** the project ships. `dotnet test` covers
  `net8.0` and `net10.0`; a change that only passes on one is not finished.
- **Public API changes need XML docs.** `GenerateDocumentationFile` is on and the
  public surface is fully documented — keep it that way.
- **Update `CHANGELOG.md`.** Keep a Changelog format, under `[Unreleased]`. Say what
  changed and why; "bump dependency" without a reason is not useful six months later.

## Dependency changes

Dependency floors are deliberate. A `PackageReference` version in a library is a
*minimum* NuGet forces on every consumer, so raising a floor is a
consumer-visible change even when nothing in the code needs it. Read
`STANDARD.md` sections 1.4 and 1.5 in `NextIteration.Standards` before changing one.

This package has no *per-target-framework* floors, which is a consequence of
those two clauses rather than an omission. Figgle, Figgle.Fonts and
Spectre.Console are all pre-1.0 and version independently of the .NET runtime, so
section 1.5 gives them a single common floor at the version actually built and
tested against — per-TFM floors would be meaningless where breaking changes land
between minors. Section 1.4's per-TFM rule covers runtime-aligned Microsoft
platform packages, and this package references none.

Minor and patch bumps arrive automatically via Dependabot and merge behind CI.
Major bumps stay open for a human — that is deliberate, not a backlog.

## Repository conventions

These repositories share a baseline defined in
[NextIteration.Standards](https://github.com/StuartMeeks/NextIteration.Standards):
build properties, test stack, CI shape, and branch protection. If a change would
deviate from it, raise that there first — a per-repo exception is a documented
entry, not a quiet difference.
