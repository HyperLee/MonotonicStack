using MonotonicStack.Algorithms;

namespace MonotonicStack.Tests.Algorithms;

public class SlidingWindowMaximumTests
{
    [Theory]
    [InlineData(new[] { 1, 3, -1, -3, 5, 3, 6, 7 }, 3, new[] { 3, 3, 5, 5, 6, 7 })]
    [InlineData(new[] { 1 }, 1, new[] { 1 })]
    [InlineData(new[] { 9, 11 }, 2, new[] { 11 })]
    [InlineData(new[] { 4, -2 }, 2, new[] { 4 })]
    [InlineData(new[] { 5, 5, 5, 5 }, 2, new[] { 5, 5, 5 })]
    [InlineData(new[] { 1, 2, 3, 4 }, 4, new[] { 4 })]
    public void Compute_KnownInputs_MatchExpected(int[] input, int k, int[] expected)
    {
        Assert.Equal(expected, SlidingWindowMaximum.Compute(input, k));
    }

    [Fact]
    public void Compute_KZero_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => SlidingWindowMaximum.Compute(new[] { 1, 2 }, 0));
    }

    [Fact]
    public void Compute_KNegative_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => SlidingWindowMaximum.Compute(new[] { 1, 2 }, -1));
    }

    [Fact]
    public void Compute_KGreaterThanLength_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => SlidingWindowMaximum.Compute(new[] { 1, 2 }, 3));
    }

    [Fact]
    public void Compute_EmptyInput_KZero_StillThrows()
    {
        // k must be >= 1, so empty input always throws.
        Assert.Throws<ArgumentOutOfRangeException>(() => SlidingWindowMaximum.Compute(ReadOnlySpan<int>.Empty, 1));
    }

    [Fact]
    public void Compute_LargeRandom_MatchesNaive()
    {
        var rng = new Random(21);
        var arr = new int[10_000];
        for (var i = 0; i < arr.Length; i++)
        {
            arr[i] = rng.Next(-10_000, 10_000);
        }

        const int k = 17;
        Assert.Equal(Naive(arr, k), SlidingWindowMaximum.Compute(arr, k));
    }

    private static int[] Naive(int[] a, int k)
    {
        var r = new int[a.Length - k + 1];
        for (var i = 0; i + k <= a.Length; i++)
        {
            var m = a[i];
            for (var j = 1; j < k; j++)
            {
                if (a[i + j] > m)
                {
                    m = a[i + j];
                }
            }

            r[i] = m;
        }

        return r;
    }
}
