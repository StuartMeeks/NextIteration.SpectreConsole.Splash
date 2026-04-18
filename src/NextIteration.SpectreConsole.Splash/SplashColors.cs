namespace NextIteration.SpectreConsole.Splash
{
    /// <summary>
    /// Gradient palette for the splash logo. A linear gradient is
    /// interpolated across the supplied <see cref="HexStops"/>; one stop
    /// produces a solid colour, two stops a simple gradient, more stops a
    /// piecewise-linear sweep.
    /// </summary>
    /// <remarks>
    /// There is no upper bound on the number of stops — compute cost is
    /// O(logo-width) regardless of stop count. That said, beyond ~5 stops
    /// the transitions wash out over a typical ~50-column logo and the
    /// visual effect diminishes. Pick the number that reads cleanly, not
    /// the maximum.
    /// </remarks>
    public sealed class SplashColors
    {
        /// <summary>
        /// The hex colour stops (e.g. <c>"#60A5FA"</c>), interpolated in
        /// order across the logo width. Always contains at least one stop.
        /// </summary>
        public IReadOnlyList<string> HexStops { get; }

        /// <summary>
        /// Constructs a palette from one or more <c>#RRGGBB</c> hex stops.
        /// </summary>
        public SplashColors(params string[] hexStops)
        {
            ArgumentNullException.ThrowIfNull(hexStops);
            if (hexStops.Length == 0)
            {
                throw new ArgumentException("At least one hex stop is required.", nameof(hexStops));
            }

            foreach (var stop in hexStops)
            {
                ValidateHex(stop);
            }

            HexStops = hexStops;
        }

        /// <summary>
        /// Neutral blue gradient (sky blue → royal blue). Chosen to read
        /// clearly on both light and dark terminals without being flashy.
        /// </summary>
        public static SplashColors Default { get; } = new("#60A5FA", "#1D4ED8");

        private static void ValidateHex(string hex)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(hex);
            if (hex.Length != 7 || hex[0] != '#')
            {
                throw new ArgumentException(
                    $"Hex colour must be in '#RRGGBB' form, got '{hex}'.", nameof(hex));
            }
            for (int i = 1; i < 7; i++)
            {
                var c = hex[i];
                var isHex = (c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f');
                if (!isHex)
                {
                    throw new ArgumentException(
                        $"Hex colour '{hex}' contains non-hex character '{c}' at position {i}.", nameof(hex));
                }
            }
        }
    }
}
