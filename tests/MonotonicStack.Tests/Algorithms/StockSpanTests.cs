using MonotonicStack.Algorithms;

namespace MonotonicStack.Tests.Algorithms;

public class StockSpanTests
{
    [Fact]
    public void Compute_EmptyInput_ReturnsEmptyArray()
    {
        Assert.Empty(StockSpan.Compute(ReadOnlySpan<int>.Empty));
    }

    [Theory]
    [InlineData(new[] { 100, 80, 60, 70, 60, 75, 85 }, new[] { 1, 1, 1, 2, 1, 4, 6 })]
    [InlineData(new[] { 1, 2, 3, 4, 5 }, new[] { 1, 2, 3, 4, 5 })]
    [InlineData(new[] { 5, 4, 3, 2, 1 }, new[] { 1, 1, 1, 1, 1 })]
    [InlineData(new[] { 7 }, new[] { 1 })]
    [InlineData(new[] { 5, 5, 5 }, new[] { 1, 2, 3 })]
    public void Compute_KnownInputs_MatchExpected(int[] input, int[] expected)
    {
        Assert.Equal(expected, StockSpan.Compute(input));
    }

    [Fact]
    public void Compute_LargeRandom_MatchesNaive()
    {
        var rng = new Random(11);
        var arr = new int[10_000];
        for (var i = 0; i < arr.Length; i++)
        {
            arr[i] = rng.Next(0, 1000);
        }

        Assert.Equal(Naive(arr), StockSpan.Compute(arr));
    }

    [Fact]
    public void StockSpanner_LeetCodeExample_ReturnsExpected()
    {
        var s = new StockSpanner();
        int[] input = [100, 80, 60, 70, 60, 75, 85];
        int[] expected = [1, 1, 1, 2, 1, 4, 6];
        for (var i = 0; i < input.Length; i++)
        {
            Assert.Equal(expected[i], s.Next(input[i]));
        }
    }

    private static int[] Naive(int[] a)
    {
        var r = new int[a.Length];
        for (var i = 0; i < a.Length; i++)
        {
            var span = 1;
            for (var j = i - 1; j >= 0 && a[j] <= a[i]; j--)
            {
                span++;
            }

            r[i] = span;
        }

        return r;
    }
}
