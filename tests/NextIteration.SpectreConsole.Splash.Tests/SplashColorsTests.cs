using NextIteration.SpectreConsole.Splash;
using Xunit;

namespace NextIteration.SpectreConsole.Splash.Tests
{
    public class SplashColorsTests
    {
        [Fact]
        public void Ctor_rejects_empty_stop_list()
        {
            Assert.Throws<ArgumentException>(() => new SplashColors());
        }

        [Fact]
        public void Ctor_rejects_null()
        {
            Assert.Throws<ArgumentNullException>(() => new SplashColors(null!));
        }

        [Theory]
        [InlineData("#GGGGGG")]   // non-hex digit
        [InlineData("#12345")]    // too short
        [InlineData("#12345678")] // too long
        [InlineData("123456")]    // missing '#'
        [InlineData("")]          // empty
        public void Ctor_rejects_malformed_hex(string bad)
        {
            Assert.Throws<ArgumentException>(() => new SplashColors(bad));
        }

        [Theory]
        [InlineData("#000000")]
        [InlineData("#ffffff")]
        [InlineData("#AbCdEf")]
        public void Ctor_accepts_both_cases_and_all_positions(string good)
        {
            var colors = new SplashColors(good);
            Assert.Single(colors.HexStops);
        }

        [Fact]
        public void Default_is_the_two_stop_blue_gradient()
        {
            Assert.Equal(2, SplashColors.Default.HexStops.Count);
            Assert.Equal("#60A5FA", SplashColors.Default.HexStops[0]);
            Assert.Equal("#1D4ED8", SplashColors.Default.HexStops[1]);
        }

        [Fact]
        public void Unlimited_stops_accepted()
        {
            var many = Enumerable.Range(0, 20).Select(_ => "#FF0000").ToArray();
            var colors = new SplashColors(many);
            Assert.Equal(20, colors.HexStops.Count);
        }
    }
}
