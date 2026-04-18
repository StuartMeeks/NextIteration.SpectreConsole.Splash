using Figgle.Fonts;
using NextIteration.SpectreConsole.Splash;
using Spectre.Console;

// Demo 1 — all defaults (Roman font, neutral blue gradient, random built-in tagline).
AnsiConsole.MarkupLine("[grey]1. defaults[/]");
SplashScreen.Show("my-cli");

// Demo 2 — custom gradient stops (three-colour sweep).
AnsiConsole.MarkupLine("[grey]2. custom three-stop gradient[/]");
SplashScreen.Show(new SplashOptions
{
    AppName = "fire-cli",
    Colors = new SplashColors("#FFEB3B", "#FF9800", "#F44336"),
});

// Demo 3 — different Figgle font + custom tagline.
AnsiConsole.MarkupLine("[grey]3. Slant font + custom tagline[/]");
SplashScreen.Show(new SplashOptions
{
    AppName = "slanted",
    Font = FiggleFonts.Slant,
    Tagline = SplashTagline.FromProvider(() => "Built with care by Next Iteration"),
});

// Demo 4 — tagline suppressed.
AnsiConsole.MarkupLine("[grey]4. no tagline[/]");
SplashScreen.Show(new SplashOptions
{
    AppName = "quiet",
    Tagline = SplashTagline.None,
});

// Demo 5 — solid colour (single stop).
AnsiConsole.MarkupLine("[grey]5. solid colour (one stop)[/]");
SplashScreen.Show(new SplashOptions
{
    AppName = "mono",
    Colors = new SplashColors("#A855F7"),
});
