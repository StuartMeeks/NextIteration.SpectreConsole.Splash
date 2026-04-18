# NextIteration.SpectreConsole.Splash

A reusable Figgle-and-Spectre.Console splash screen for .NET CLIs.

- Configurable app name, Figgle font, gradient palette, and tagline strategy.
- Piecewise-linear gradient across an arbitrary number of `#RRGGBB` stops.
- Built-in pool of ~300 developer-culture taglines, or supply your own.
- Performance-first: the entire splash is assembled as one markup string and
  emitted via a single `AnsiConsole.Markup` call.

## Install

```bash
dotnet add package NextIteration.SpectreConsole.Splash
```

## Usage

```csharp
using NextIteration.SpectreConsole.Splash;

// One-line: defaults (Roman font, neutral blue gradient, random tagline).
SplashScreen.Show("my-cli");

// Fully configured.
SplashScreen.Show(new SplashOptions
{
    AppName = "my-cli",
    Font = Figgle.Fonts.FiggleFonts.Slant,
    Colors = new SplashColors("#FFEB3B", "#FF9800", "#F44336"),
    Tagline = SplashTagline.FromProvider(() => $"v{AppVersion.Current}"),
});

// Suppress the tagline.
SplashScreen.Show(new SplashOptions
{
    AppName = "quiet",
    Tagline = SplashTagline.None,
});
```

## Defaults at a glance

| Property  | Default                                                   |
| --------- | --------------------------------------------------------- |
| `Font`    | `FiggleFonts.Roman`                                       |
| `Colors`  | `#60A5FA` → `#1D4ED8` (neutral blue, reads on light/dark) |
| `Tagline` | Random pick from the built-in quote pool (~300 entries)   |

## Architecture

| File                    | Role                                                                            |
| ----------------------- | ------------------------------------------------------------------------------- |
| `SplashScreen.cs`       | Public entry point (`Show`).                                                    |
| `SplashOptions.cs`      | Per-call configuration. Only `AppName` is required.                             |
| `SplashColors.cs`       | Gradient palette; validates `#RRGGBB` at construction.                          |
| `SplashTagline.cs`      | Strategy for the sub-logo tagline (`None` / `RandomBuiltIn` / `FromProvider`).  |
| `Internal/Gradient.cs`  | Pure-function piecewise-linear interpolation, O(width) regardless of N stops.  |
| `Internal/Renderer.cs`  | Builds the full splash as one Spectre.Console markup string.                    |
| `Internal/Quotes.cs`    | Built-in tagline pool.                                                          |

## Performance notes

The original internal precursor of this library emitted one
`AnsiConsole.Write(char)` call per visible logo character — ~250 calls for
a typical Roman logo, each flushing its own ANSI colour-change escape.
This rewrite batches the entire splash (logo + tagline) into a single
Spectre markup string and does one `AnsiConsole.Markup` call. Measured
cold start on a 64×7 Roman logo: ~60 ms before, <10 ms after.

Additional wins:
- `FiggleFonts.Roman.Render` runs on the first `Show` call, not at
  class-load.
- Line endings normalise on `\n` and trim trailing `\r` — no more
  Windows-only logo splits breaking silently on Unix.
- Space characters skip the colour-escape wrapper entirely (saves ~14
  markup chars per space).

## License

MIT — see [LICENSE](./LICENSE).
