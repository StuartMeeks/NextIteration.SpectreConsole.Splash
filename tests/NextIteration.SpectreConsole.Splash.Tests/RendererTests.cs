using NextIteration.SpectreConsole.Splash.Internal;
using Spectre.Console;
using Xunit;

namespace NextIteration.SpectreConsole.Splash.Tests
{
    public class RendererTests
    {
        private static readonly Color[] TrivialGradient = [new Color(255, 0, 0), new Color(0, 255, 0)];

        [Fact]
        public void Empty_logo_still_renders_something()
        {
            var output = Renderer.Render(string.Empty, TrivialGradient, tagline: null);

            Assert.NotNull(output);
            Assert.NotEmpty(output);
        }

        [Fact]
        public void Null_tagline_does_not_emit_tagline_lines()
        {
            var output = Renderer.Render("ab", TrivialGradient, tagline: null);
            // No markup escape sequence should contain our placeholder.
            Assert.DoesNotContain("tagline", output);
        }

        [Fact]
        public void Whitespace_tagline_is_treated_as_absent()
        {
            var outWithNull = Renderer.Render("ab", TrivialGradient, tagline: null);
            var outWithWs = Renderer.Render("ab", TrivialGradient, tagline: "   ");
            Assert.Equal(outWithNull, outWithWs);
        }

        [Fact]
        public void Logo_characters_are_wrapped_in_spectre_hex_markup()
        {
            var output = Renderer.Render("X", TrivialGradient, tagline: null);

            // Every visible glyph gets a `[#RRGGBB]X[/]` wrapper.
            Assert.Matches(@"\[#[0-9A-F]{6}\]X\[/\]", output);
        }

        [Fact]
        public void Spaces_in_logo_are_not_wrapped_in_markup()
        {
            // A bare space shouldn't pick up colour escapes — saves markup volume.
            var output = Renderer.Render(" ", TrivialGradient, tagline: null);

            // Still contains the space, but no `[#...]` wrapping an empty body.
            Assert.Contains(" ", output);
            Assert.DoesNotMatch(@"\[#[0-9A-F]{6}\]\s\[/\]", output);
        }

        [Fact]
        public void Bracket_characters_in_logo_are_escaped_for_markup_parser()
        {
            // Figgle glyphs can contain `[` and `]`. Markup parser treats
            // `[[` and `]]` as literal brackets. Without escaping, a logo
            // row containing `[` would be parsed as an unterminated tag.
            var output = Renderer.Render("[]", TrivialGradient, tagline: null);

            Assert.Contains("[[", output);
            Assert.Contains("]]", output);
        }

        [Fact]
        public void CRLF_and_LF_line_endings_produce_same_rendering()
        {
            var lf = Renderer.Render("a\nb", TrivialGradient, tagline: null);
            var crlf = Renderer.Render("a\r\nb", TrivialGradient, tagline: null);
            Assert.Equal(lf, crlf);
        }

        [Fact]
        public void Tagline_text_appears_in_output()
        {
            // Use a wide logo so the tagline isn't truncated by width.
            var wideLogo = new string('X', 60);
            var output = Renderer.Render(wideLogo, TrivialGradient, tagline: "hello world");

            Assert.Contains("hello", output);
            Assert.Contains("world", output);
        }
    }
}
