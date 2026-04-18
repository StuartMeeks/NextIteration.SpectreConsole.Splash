using Figgle;
using Figgle.Fonts;

namespace NextIteration.SpectreConsole.Splash
{
    /// <summary>
    /// Configuration for a single <see cref="SplashScreen.Show(SplashOptions)"/>
    /// call. Only <see cref="AppName"/> is required; everything else has a
    /// sensible default.
    /// </summary>
    public sealed class SplashOptions
    {
        /// <summary>
        /// The text rendered as the splash logo (via <see cref="Font"/>).
        /// Typically the CLI's short name, e.g. <c>"my-cli"</c>.
        /// </summary>
        public required string AppName { get; init; }

        /// <summary>
        /// The Figgle font used to render <see cref="AppName"/>.
        /// Default: <see cref="FiggleFonts.Roman"/>.
        /// </summary>
        public FiggleFont Font { get; init; } = FiggleFonts.Roman;

        /// <summary>
        /// Gradient palette applied across the logo horizontally.
        /// Default: <see cref="SplashColors.Default"/> (neutral blue).
        /// </summary>
        public SplashColors Colors { get; init; } = SplashColors.Default;

        /// <summary>
        /// Tagline strategy. Default: a random quote from the library's
        /// built-in list. Set to <see cref="SplashTagline.None"/> to
        /// suppress the tagline, or
        /// <see cref="SplashTagline.FromProvider(Func{string?})"/> to
        /// supply your own.
        /// </summary>
        public SplashTagline Tagline { get; init; } = SplashTagline.RandomBuiltIn;
    }
}
