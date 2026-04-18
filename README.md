# NextIteration.SpectreConsole.Splash

A reusable Figgle-and-Spectre.Console splash screen for .NET CLIs.

- Configurable app name, Figgle font, gradient palette, and tagline strategy.
- Piecewise-linear gradient across an arbitrary number of `#RRGGBB` stops.
- Built-in pool of ~300 developer-culture taglines, or supply your own.
- Performance-first: the entire splash is assembled as one markup string and
  emitted via a single `AnsiConsole.Markup` call.

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

## Consumption

This package is **not published to NuGet**. Consume it as a git submodule
plus a `<ProjectReference>`:

```bash
cd /path/to/your-consumer-solution
git submodule add https://github.com/StuartMeeks/NextIteration.SpectreConsole.Splash.git external/NextIteration.SpectreConsole.Splash
```

Then in the consuming csproj:

```xml
<ItemGroup>
  <ProjectReference Include="..\external\NextIteration.SpectreConsole.Splash\src\NextIteration.SpectreConsole.Splash\NextIteration.SpectreConsole.Splash.csproj" />
</ItemGroup>
```

Clone the consumer with submodules:

```bash
git clone --recursive https://github.com/you/your-consumer.git
# or, post-hoc:
git submodule update --init --recursive
```

## Why not NuGet?

This is an opinionated splash helper with small surface area and no
ABI-stability promises. Git-submodule consumption keeps the source in
your consumers' hands — step-through debugging, easy patching, and zero
package-resolution friction. If that changes, publishing to NuGet is a
trivial follow-up.

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

## License

MIT — see [LICENSE](./LICENSE).
