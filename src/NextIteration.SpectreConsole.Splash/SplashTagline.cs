using NextIteration.SpectreConsole.Splash.Internal;

namespace NextIteration.SpectreConsole.Splash
{
    /// <summary>
    /// Tagline strategy for the splash screen.
    /// <list type="bullet">
    ///   <item><see cref="None"/> — render no tagline at all.</item>
    ///   <item><see cref="RandomBuiltIn"/> — pick a random quote from the library's built-in list (~200 quotes).</item>
    ///   <item><see cref="FromProvider"/> — supply your own provider callback.</item>
    /// </list>
    /// </summary>
    public abstract class SplashTagline
    {
        private protected SplashTagline() { }

        /// <summary>Do not render a tagline.</summary>
        public static SplashTagline None { get; } = new NoneTagline();

        /// <summary>Render a random quote from the library's built-in list.</summary>
        public static SplashTagline RandomBuiltIn { get; } = new BuiltInTagline();

        /// <summary>
        /// Render whatever <paramref name="provider"/> returns. Return
        /// <see langword="null"/> or an empty string from the provider to
        /// suppress the tagline on a given run.
        /// </summary>
        public static SplashTagline FromProvider(Func<string?> provider)
        {
            ArgumentNullException.ThrowIfNull(provider);
            return new ProviderTagline(provider);
        }

        /// <summary>
        /// Returns the tagline text for this strategy, or <see langword="null"/>
        /// if nothing should be rendered.
        /// </summary>
        internal abstract string? Resolve();

        private sealed class NoneTagline : SplashTagline
        {
            internal override string? Resolve() => null;
        }

        private sealed class BuiltInTagline : SplashTagline
        {
            internal override string? Resolve() => Quotes.Random();
        }

        private sealed class ProviderTagline(Func<string?> provider) : SplashTagline
        {
            internal override string? Resolve() => provider();
        }
    }
}
