using Spectre.Console;

namespace NextIteration.SpectreConsole.Splash.Internal
{
    /// <summary>
    /// Linear-interpolated colour gradient across N stops, sampled to M
    /// output columns. Pure function; no allocations beyond the result
    /// array.
    /// </summary>
    internal static class Gradient
    {
        /// <summary>
        /// Returns <paramref name="width"/> colours sampled from a
        /// piecewise-linear gradient across <paramref name="hexStops"/>.
        /// Asserts <c>hexStops.Count &gt;= 1</c> (validated at
        /// <see cref="SplashColors"/> construction time).
        /// </summary>
        public static Color[] Generate(IReadOnlyList<string> hexStops, int width)
        {
            if (width <= 0) return [];

            // Convert hex → RGB once.
            var stops = new (byte R, byte G, byte B)[hexStops.Count];
            for (int i = 0; i < hexStops.Count; i++)
            {
                stops[i] = HexToRgb(hexStops[i]);
            }

            var result = new Color[width];

            // Solid colour — one stop is a shortcut.
            if (stops.Length == 1)
            {
                var c = new Color(stops[0].R, stops[0].G, stops[0].B);
                Array.Fill(result, c);
                return result;
            }

            // Width == 1 special-case: just use the first stop.
            if (width == 1)
            {
                result[0] = new Color(stops[0].R, stops[0].G, stops[0].B);
                return result;
            }

            // Piecewise-linear across all stops. The parameter `t` goes
            // from 0 to (stops.Length - 1) as we sweep across `width`
            // positions. The segment index is floor(t); the in-segment
            // position is the fractional part.
            var segments = stops.Length - 1;
            var step = (float)segments / (width - 1);

            for (int i = 0; i < width; i++)
            {
                var t = i * step;
                var segment = (int)t;
                if (segment >= segments) segment = segments - 1;
                var fraction = t - segment;

                var a = stops[segment];
                var b = stops[segment + 1];

                var r = (byte)(a.R + fraction * (b.R - a.R));
                var g = (byte)(a.G + fraction * (b.G - a.G));
                var bl = (byte)(a.B + fraction * (b.B - a.B));

                result[i] = new Color(r, g, bl);
            }

            return result;
        }

        private static (byte R, byte G, byte B) HexToRgb(string hex)
        {
            // Input is validated in SplashColors so we don't re-check here.
            var r = (byte)((CharToHex(hex[1]) << 4) | CharToHex(hex[2]));
            var g = (byte)((CharToHex(hex[3]) << 4) | CharToHex(hex[4]));
            var b = (byte)((CharToHex(hex[5]) << 4) | CharToHex(hex[6]));
            return (r, g, b);
        }

        private static int CharToHex(char c) => c switch
        {
            >= '0' and <= '9' => c - '0',
            >= 'A' and <= 'F' => c - 'A' + 10,
            >= 'a' and <= 'f' => c - 'a' + 10,
            _ => 0, // Unreachable — SplashColors validates at construction.
        };
    }
}
