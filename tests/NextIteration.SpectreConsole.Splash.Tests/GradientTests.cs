using NextIteration.SpectreConsole.Splash.Internal;
using Xunit;

namespace NextIteration.SpectreConsole.Splash.Tests
{
    public class GradientTests
    {
        [Fact]
        public void Width_zero_returns_empty()
        {
            var result = Gradient.Generate(["#FF0000"], 0);
            Assert.Empty(result);
        }

        [Fact]
        public void Single_stop_produces_solid_colour()
        {
            var result = Gradient.Generate(["#FF8000"], 10);

            Assert.Equal(10, result.Length);
            foreach (var c in result)
            {
                Assert.Equal(255, c.R);
                Assert.Equal(128, c.G);
                Assert.Equal(0, c.B);
            }
        }

        [Fact]
        public void Two_stop_gradient_starts_at_a_ends_at_b()
        {
            var result = Gradient.Generate(["#000000", "#FFFFFF"], 10);

            // First sample == stop[0], last sample == stop[1]
            Assert.Equal(0, result[0].R);
            Assert.Equal(0, result[0].G);
            Assert.Equal(0, result[0].B);

            Assert.Equal(255, result[^1].R);
            Assert.Equal(255, result[^1].G);
            Assert.Equal(255, result[^1].B);
        }

        [Fact]
        public void Two_stop_gradient_middle_is_roughly_midpoint()
        {
            var result = Gradient.Generate(["#000000", "#FFFFFF"], 11);
            var mid = result[5];

            // Midpoint of 0→255 is 127.5; allow ±2 tolerance for integer casts.
            Assert.InRange(mid.R, 125, 130);
            Assert.InRange(mid.G, 125, 130);
            Assert.InRange(mid.B, 125, 130);
        }

        [Fact]
        public void Three_stop_gradient_hits_middle_stop_at_centre()
        {
            // 0 → 50% → 100% maps to stops[0], stops[1], stops[2].
            var result = Gradient.Generate(["#000000", "#FF0000", "#0000FF"], 21);

            Assert.Equal(0, result[0].R);
            Assert.Equal(0, result[0].G);
            Assert.Equal(0, result[0].B);

            var mid = result[10];
            Assert.InRange(mid.R, 250, 255);
            Assert.InRange(mid.G, 0, 5);
            Assert.InRange(mid.B, 0, 5);

            Assert.InRange(result[^1].R, 0, 5);
            Assert.InRange(result[^1].G, 0, 5);
            Assert.InRange(result[^1].B, 250, 255);
        }

        [Fact]
        public void Width_one_returns_first_stop()
        {
            var result = Gradient.Generate(["#123456", "#FFFFFF"], 1);

            Assert.Single(result);
            Assert.Equal(0x12, result[0].R);
            Assert.Equal(0x34, result[0].G);
            Assert.Equal(0x56, result[0].B);
        }

        [Fact]
        public void Large_stop_count_still_linear_time()
        {
            // Regression guard: 20 stops, 200 cols = 4000 ops, must still
            // produce coherent output (endpoints anchored).
            var stops = new string[20];
            for (int i = 0; i < 20; i++) stops[i] = i % 2 == 0 ? "#000000" : "#FFFFFF";

            var result = Gradient.Generate(stops, 200);

            Assert.Equal(200, result.Length);
            Assert.Equal(0, result[0].R);
            Assert.Equal(255, result[^1].R); // 20 stops, last is white.
        }
    }
}
