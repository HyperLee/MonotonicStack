using MonotonicStack.Algorithms;

namespace MonotonicStack.Tests.Algorithms;

public class PreviousGreaterElementTests
{
    [Fact]
    public void Compute_EmptyInput_ReturnsEmptyArray()
    {
        Assert.Empty(PreviousGreaterElement.Compute(ReadOnlySpan<int>.Empty));
    }

    [Theory]
    [InlineData(new[] { 2, 1, 2, 4, 3, 1 }, new[] { -1, 2, -1, -1, 4, 3 })]
    [InlineData(new[] { 1, 2, 3, 4 }, new[] { -1, -1, -1, -1 })]
    [InlineData(new[] { 4, 3, 2, 1 }, new[] { -1, 4, 3, 2 })]
    [InlineData(new[] { 5, 5, 5 }, new[] { -1, -1, -1 })]
    public void Compute_KnownInputs_MatchExpected(int[] input, int[] expected)
    {
        Assert.Equal(expected, PreviousGreaterElement.Compute(input));
    }

    [Fact]
    public void Compute_LargeRandom_MatchesNaive()
    {
        var rng = new Random(7);
        var arr = new int[10_000];
        for (var i = 0; i < arr.Length; i++)
        {
            arr[i] = rng.Next(-1000, 1000);
        }

        Assert.Equal(Naive(arr), PreviousGreaterElement.Compute(arr));
    }

    private static int[] Naive(int[] a)
    {
        var r = new int[a.Length];
        for (var i = 0; i < a.Length; i++)
        {
            r[i] = -1;
            for (var j = i - 1; j >= 0; j--)
            {
                if (a[j] > a[i])
                {
                    r[i] = a[j];
                    break;
                }
            }
        }

        return r;
    }
}
