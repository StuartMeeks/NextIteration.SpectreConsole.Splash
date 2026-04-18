using NextIteration.SpectreConsole.Splash.Internal;
using Xunit;

namespace NextIteration.SpectreConsole.Splash.Tests
{
    public class QuotesTests
    {
        [Fact]
        public void Pool_has_at_least_100_entries()
        {
            // Rough lower bound — the whole point of the built-in pool is
            // that the same quote shouldn't appear twice in quick succession.
            Assert.True(Quotes.Count >= 100, $"Expected >= 100 quotes, got {Quotes.Count}");
        }

        [Fact]
        public void No_null_or_whitespace_quotes()
        {
            foreach (var q in Quotes.All)
            {
                Assert.False(string.IsNullOrWhiteSpace(q));
            }
        }

        [Fact]
        public void Random_returns_one_of_the_pool_entries()
        {
            var picked = Quotes.Random();
            Assert.Contains(picked, Quotes.All.ToArray());
        }

        [Fact]
        public void Random_eventually_returns_different_values()
        {
            // Not strictly guaranteed, but 500 calls from a ~300-entry pool
            // with Random.Shared collisions only would be astronomically rare.
            var seen = new HashSet<string>();
            for (int i = 0; i < 500; i++) seen.Add(Quotes.Random());

            Assert.True(seen.Count > 1, "Random() returned the same value 500 times in a row");
        }
    }
}
