using MonotonicStack.Algorithms;

namespace MonotonicStack.Tests.Algorithms;

public class LargestRectangleInHistogramTests
{
    [Fact]
    public void Compute_EmptyInput_ReturnsZero()
    {
        Assert.Equal(0, LargestRectangleInHistogram.Compute(ReadOnlySpan<int>.Empty));
    }

    [Theory]
    [InlineData(new[] { 2, 1, 5, 6, 2, 3 }, 10)]
    [InlineData(new[] { 2, 4 }, 4)]
    [InlineData(new[] { 1, 1, 1, 1 }, 4)]
    [InlineData(new[] { 5 }, 5)]
    [InlineData(new[] { 0 }, 0)]
    [InlineData(new[] { 1, 2, 3, 4, 5 }, 9)]
    [InlineData(new[] { 5, 4, 3, 2, 1 }, 9)]
    public void Compute_KnownInputs_MatchExpected(int[] input, int expected)
    {
        Assert.Equal(expected, LargestRectangleInHistogram.Compute(input));
    }

    [Fact]
    public void Compute_NegativeHeight_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => LargestRectangleInHistogram.Compute(new[] { 1, -1, 2 }));
    }

    [Fact]
    public void Compute_LargeRandom_MatchesNaive()
    {
        var rng = new Random(13);
        var arr = new int[2_000];
        for (var i = 0; i < arr.Length; i++)
        {
            arr[i] = rng.Next(0, 100);
        }

        Assert.Equal(Naive(arr), LargestRectangleInHistogram.Compute(arr));
    }

    private static int Naive(int[] h)
    {
        var max = 0;
        for (var i = 0; i < h.Length; i++)
        {
            var minH = h[i];
            for (var j = i; j < h.Length; j++)
            {
                minH = Math.Min(minH, h[j]);
                max = Math.Max(max, minH * (j - i + 1));
            }
        }

        return max;
    }
}
