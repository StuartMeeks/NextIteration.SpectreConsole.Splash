# CLAUDE.md — NextIteration.SpectreConsole.Splash

## This package

A Figgle-and-Spectre.Console splash screen for .NET CLIs. A consumer calls
`SplashScreen.Show("my-cli")` for the defaults, or passes a `SplashOptions` to choose the
Figgle font, a `SplashColors` gradient palette of `#RRGGBB` stops, and a `SplashTagline`
strategy (`None`, `RandomBuiltIn` from a ~300-entry built-in pool, or `FromProvider` with a
callback). There is no DI registration and no command branch — it is one static entry point
with no state. Nothing here consumes another package in the estate, and nothing in the estate
consumes it.

## Things that are easy to get wrong here

- **The single `AnsiConsole.Markup` call is the whole point of the library.** `Renderer`
  assembles logo and tagline into one markup string and `SplashScreen.Show` emits it with one
  call. The precursor wrote one `AnsiConsole.Write(char)` per character — ~250 calls for a
  typical Roman logo, each flushing its own ANSI colour escape — and measured ~60 ms against
  under 10 ms for the batched path. Any change that writes incrementally, or that renders
  through a Spectre widget instead of a markup string, gives that back. The figure is quoted
  in the README and in the `SplashScreen` XML docs, so a rewrite has to re-measure both.
- **Markup escaping is load-bearing, not defensive tidiness.** Figgle glyphs contain `[` and
  `]`, and Spectre parses those as markup control characters. This is not hypothetical in the
  default font: in Roman, the glyphs for `w`, `K`, `2`, `3`, `5` and `6` all contain a
  bracket, so an app called `wget` or `k8s` hits it immediately. `RenderLogo` doubles them
  inline per character; `RenderTagline` escapes through `EscapeMarkup` *after* padding,
  because padding can only add spaces but reordering the two would be easy and wrong.
  Dropping either escape does not throw — it silently corrupts the logo or swallows part of
  the tagline as a bogus colour tag.
- **Line-ending normalisation is implemented twice, and the two must agree.**
  `SplashScreen.MaxLineLength` scans for `\n` and discounts a preceding `\r` to compute the
  width the gradient is generated at; `Renderer.Render` independently splits on `\n` and
  `TrimEnd('\r')`s to compute the width the logo is rendered at. Both handle `\r` because the
  code treats Figgle's line endings as platform-dependent (on Linux with Figgle 0.6.6 the
  output is `\n`-only, so the Windows leg of the matrix is what actually exercises the other
  branch). If they ever disagree the gradient is
  sampled to the wrong width and the sweep no longer reaches the end of the logo — a visual
  bug with no exception and no failing test unless one is written for the pair together.
- **`Gradient` deliberately trusts its input; `SplashColors` is the only validator.**
  `HexToRgb` and `CharToHex` do no checking at all — `CharToHex` maps anything unrecognised
  to `0` with an "unreachable" comment — because `SplashColors`' constructor has already
  rejected anything that is not `#RRGGBB`. That invariant is the contract between the two
  types. Any new path that reaches `Gradient.Generate` without going through a constructed
  `SplashColors` silently renders wrong colours instead of throwing.
- **Spaces skip the colour escape entirely.** `RenderLogo` appends a bare `' '` rather than
  wrapping it, which saves ~14 markup characters per space and is a large fraction of a
  Figgle logo. It is safe only because a space has no visible foreground; a change that gives
  the splash a background colour makes it wrong.
- **The font renders on first `Show`, not at class load.** `FiggleFonts.Roman` is touched
  lazily via the `SplashOptions` default. Hoisting it into a static initialiser moves that
  cost onto every consumer that loads the assembly without showing a splash.

## Repository baseline

This repo conforms to
[NextIteration.Standards](https://github.com/StuartMeeks/NextIteration.Standards).
Build properties, test stack, CI shape, and branch protection are defined there, not
here. Before changing any of those, read `STANDARD.md`; if this repo needs to deviate,
that is an `EXCEPTIONS.md` entry in the standards repo, not a local difference.

## Non-negotiables

- **The build must be clean.** `TreatWarningsAsErrors` is on and analyzers run at
  `latest`. A warning is a build failure.
- **Tests must pass on every shipped target framework** (`net8.0` and `net10.0`). A change
  that only passes on one is not finished. Shipping a target you do not test is a defect,
  not a scoping decision.
- **Dependency floors are deliberate.** A `PackageReference` version in a library is a
  *minimum* NuGet forces on every consumer, so raising a floor is a consumer-visible change
  even when nothing in the code needs it. Never raise one to silence a warning. This repo
  has no *per-TFM* floors, and that is not an oversight: Figgle, Figgle.Fonts and
  Spectre.Console are all pre-1.0 and version independently of the .NET runtime, so
  `STANDARD.md` §1.5 gives them a single common floor at the version actually built and
  tested against. §1.4's per-TFM rule applies only to runtime-aligned Microsoft platform
  packages, of which this repo references none.
- **Public API changes need XML docs.** `GenerateDocumentationFile` is on and the public
  surface is fully documented.
- **Update `CHANGELOG.md`** under `[Unreleased]`, saying what changed and why.

## Dependabot

Minor and patch updates auto-merge behind CI. Major updates stay open for a human — that
is deliberate, not a backlog to clear. `dependabot.yml` here has **no `ignore` block**, and
adding one would be wrong: §4.10 scopes that list to packages carrying per-TFM floors, and
this repo has none, so every major bump it files is genuinely reviewable. `audit-drift.sh`
checks the `ignore` set against the repo's actual per-TFM floors, so an inherited entry
fails the audit rather than passing quietly.

## After opening a pull request

Watch CI to completion, report the real check results, then **offer to merge** in the same
message. Do not stop silently and wait to be asked.

- If branch protection blocks the merge, say so and offer `gh pr merge --admin`. These
  repos require a code-owner review only the maintainer can give, which is why `--admin` is
  the tool — but that mechanic is not the reason the offer is wanted. The reason is simply
  that the maintainer has grown comfortable delegating this to an agent, so treat the
  latest instruction as authoritative over this file.
- **Merge only on an explicit yes.** The offer is pre-approved; the action is not.
- Never offer while checks are failing or still running. Report that state instead.
- Report the checks that actually ran. A skipped check is not a passing check, and branch
  protection treats them differently from how they read in a summary.

## CI

The single required status check is `ci` — an aggregating gate over `build` and `test`.
Renaming those jobs is safe; the ruleset never names them. Do not make them required
checks directly.

**`ci.yml` is not yours to edit freely.** `STANDARD.md` §3.0.1 requires its non-comment
content to match `templates/.github/workflows/ci.yml` in the standards repo exactly, apart
from the tag glob and steps carrying an `EXCEPTIONS.md` entry. This repo has no such entry,
so the only permitted difference is `tags: [ 'v*' ]` and the header comment.
`audit-drift.sh` checks this. Change the template first, then every repo — never this file
alone.

The `test` matrix runs Linux, Windows and macOS. Nothing here touches the filesystem or an
OS store, so no platform has its own backend — but Figgle's line endings and Spectre's
console capability detection both differ by platform, and §3.1.1 removes platforms by
exception only. Do not drop a leg to save CI minutes.
