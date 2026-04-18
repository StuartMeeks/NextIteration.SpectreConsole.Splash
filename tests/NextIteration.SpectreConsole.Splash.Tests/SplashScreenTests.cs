using NextIteration.SpectreConsole.Splash;
using Spectre.Console;
using Spectre.Console.Testing;
using Xunit;

namespace NextIteration.SpectreConsole.Splash.Tests
{
    public class SplashScreenTests
    {
        [Fact]
        public void Show_with_null_options_throws()
        {
            Assert.Throws<ArgumentNullException>(() => SplashScreen.Show((SplashOptions)null!));
        }

        [Fact]
        public void Show_with_empty_appName_throws()
        {
            Assert.Throws<ArgumentException>(() => SplashScreen.Show(""));
            Assert.Throws<ArgumentException>(() => SplashScreen.Show("   "));
        }

        [Fact]
        public void Show_writes_to_AnsiConsole()
        {
            // Redirecting AnsiConsole.Console to a test console lets us
            // confirm the single-Markup call actually produces output.
            var prev = AnsiConsole.Console;
            var test = new TestConsole();
            AnsiConsole.Console = test;
            try
            {
                SplashScreen.Show(new SplashOptions
                {
                    AppName = "hi",
                    Tagline = SplashTagline.None,
                });
            }
            finally
            {
                AnsiConsole.Console = prev;
            }

            // Figgle-rendered "hi" contains lowercase glyph art — at
            // minimum the console output should be non-empty.
            Assert.False(string.IsNullOrWhiteSpace(test.Output));
        }

        [Fact]
        public void Show_with_custom_tagline_emits_it()
        {
            var prev = AnsiConsole.Console;
            var test = new TestConsole().Width(120);
            AnsiConsole.Console = test;
            try
            {
                SplashScreen.Show(new SplashOptions
                {
                    AppName = "app",
                    Tagline = SplashTagline.FromProvider(() => "custom tagline here"),
                });
            }
            finally
            {
                AnsiConsole.Console = prev;
            }

            Assert.Contains("custom", test.Output);
        }
    }
}
