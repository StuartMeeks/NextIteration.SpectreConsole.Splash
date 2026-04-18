using NextIteration.SpectreConsole.Splash;
using Xunit;

namespace NextIteration.SpectreConsole.Splash.Tests
{
    public class SplashTaglineTests
    {
        [Fact]
        public void None_resolves_to_null()
        {
            Assert.Null(SplashTagline.None.Resolve());
        }

        [Fact]
        public void RandomBuiltIn_resolves_to_a_non_empty_string()
        {
            var result = SplashTagline.RandomBuiltIn.Resolve();
            Assert.False(string.IsNullOrWhiteSpace(result));
        }

        [Fact]
        public void FromProvider_invokes_callback_every_resolve()
        {
            var count = 0;
            var tagline = SplashTagline.FromProvider(() => $"call-{++count}");

            Assert.Equal("call-1", tagline.Resolve());
            Assert.Equal("call-2", tagline.Resolve());
        }

        [Fact]
        public void FromProvider_allows_null_result()
        {
            var tagline = SplashTagline.FromProvider(() => null);
            Assert.Null(tagline.Resolve());
        }

        [Fact]
        public void FromProvider_rejects_null_callback()
        {
            Assert.Throws<ArgumentNullException>(() => SplashTagline.FromProvider(null!));
        }
    }
}
