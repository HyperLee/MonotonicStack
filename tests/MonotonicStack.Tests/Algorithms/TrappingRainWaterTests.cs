using MonotonicStack.Algorithms;

namespace MonotonicStack.Tests.Algorithms;

public class TrappingRainWaterTests
{
    [Fact]
    public void Compute_EmptyInput_ReturnsZero()
    {
        Assert.Equal(0, TrappingRainWater.Compute(ReadOnlySpan<int>.Empty));
    }

    [Theory]
    [InlineData(new[] { 0, 1, 0, 2, 1, 0, 1, 3, 2, 1, 2, 1 }, 6)]
    [InlineData(new[] { 4, 2, 0, 3, 2, 5 }, 9)]
    [InlineData(new[] { 1, 2, 3, 4 }, 0)]
    [InlineData(new[] { 4, 3, 2, 1 }, 0)]
    [InlineData(new[] { 5 }, 0)]
    [InlineData(new[] { 5, 5, 5 }, 0)]
    [InlineData(new[] { 3, 0, 3 }, 3)]
    public void Compute_KnownInputs_MatchExpected(int[] input, int expected)
    {
        Assert.Equal(expected, TrappingRainWater.Compute(input));
    }

    [Fact]
    public void Compute_NegativeHeight_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => TrappingRainWater.Compute(new[] { 1, -1, 2 }));
    }

    [Fact]
    public void Compute_LargeRandom_MatchesNaive()
    {
        var rng = new Random(3);
        var arr = new int[5_000];
        for (var i = 0; i < arr.Length; i++)
        {
            arr[i] = rng.Next(0, 100);
        }

        Assert.Equal(Naive(arr), TrappingRainWater.Compute(arr));
    }

    private static int Naive(int[] h)
    {
        if (h.Length == 0)
        {
            return 0;
        }

        var n = h.Length;
        var lmax = new int[n];
        var rmax = new int[n];
        lmax[0] = h[0];
        for (var i = 1; i < n; i++)
        {
            lmax[i] = Math.Max(lmax[i - 1], h[i]);
        }

        rmax[n - 1] = h[n - 1];
        for (var i = n - 2; i >= 0; i--)
        {
            rmax[i] = Math.Max(rmax[i + 1], h[i]);
        }

        var total = 0;
        for (var i = 0; i < n; i++)
        {
            total += Math.Min(lmax[i], rmax[i]) - h[i];
        }

        return total;
    }
}
