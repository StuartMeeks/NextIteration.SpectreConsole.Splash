using NextIteration.SpectreConsole.Splash.Internal;
using Spectre.Console;

namespace NextIteration.SpectreConsole.Splash
{
    /// <summary>
    /// Renders a Figgle ASCII-art splash screen with a horizontal colour
    /// gradient and an optional tagline. Two one-shot entry points:
    /// <list type="bullet">
    ///   <item><see cref="Show(string)"/> — sugar for the default options.</item>
    ///   <item><see cref="Show(SplashOptions)"/> — full configurability.</item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// <para>
    /// Performance: the entire splash is assembled as a single markup
    /// string and emitted via one <see cref="AnsiConsole.Markup(string)"/>
    /// call. That replaces the previous per-character write pattern,
    /// which was dominated by ANSI-escape flushing — on a typical
    /// 64-column, 6-row Roman logo the per-char path measured ~60 ms
    /// versus &lt;10 ms for the batched path.
    /// </para>
    /// <para>
    /// Thread safety: <c>Show</c> is safe to call concurrently (no shared
    /// mutable state), though interleaving two splashes on the same
    /// console gives you a garbled mess, so don't.
    /// </para>
    /// </remarks>
    public static class SplashScreen
    {
        /// <summary>
        /// Renders the splash for the given <paramref name="appName"/> using
        /// all defaults (Roman font, neutral blue gradient, random built-in
        /// tagline).
        /// </summary>
        public static void Show(string appName) =>
            Show(new SplashOptions { AppName = appName });

        /// <summary>
        /// Renders the splash using the supplied <paramref name="options"/>.
        /// </summary>
        public static void Show(SplashOptions options)
        {
            ArgumentNullException.ThrowIfNull(options);
            ArgumentException.ThrowIfNullOrWhiteSpace(options.AppName);

            var logo = options.Font.Render(options.AppName);
            var maxLineLength = MaxLineLength(logo);
            var gradient = Gradient.Generate(options.Colors.HexStops, maxLineLength);
            var tagline = options.Tagline.Resolve();

            var markup = Renderer.Render(logo, gradient, tagline);

            AnsiConsole.Markup(markup);
        }

        private static int MaxLineLength(string logo)
        {
            var max = 0;
            var start = 0;
            for (int i = 0; i < logo.Length; i++)
            {
                if (logo[i] == '\n')
                {
                    var end = i > start && logo[i - 1] == '\r' ? i - 1 : i;
                    var len = end - start;
                    if (len > max) max = len;
                    start = i + 1;
                }
            }
            // Trailing line without newline.
            var tailLen = logo.Length - start;
            if (tailLen > max) max = tailLen;
            return max;
        }
    }
}
