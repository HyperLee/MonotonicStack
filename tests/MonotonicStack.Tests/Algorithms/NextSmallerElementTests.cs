using MonotonicStack.Algorithms;

namespace MonotonicStack.Tests.Algorithms;

public class NextSmallerElementTests
{
    [Fact]
    public void Compute_EmptyInput_ReturnsEmptyArray()
    {
        Assert.Empty(NextSmallerElement.Compute(ReadOnlySpan<int>.Empty));
    }

    [Theory]
    [InlineData(new[] { 4, 8, 5, 2, 25 }, new[] { 2, 5, 2, -1, -1 })]
    [InlineData(new[] { 1, 2, 3 }, new[] { -1, -1, -1 })]
    [InlineData(new[] { 3, 2, 1 }, new[] { 2, 1, -1 })]
    [InlineData(new[] { 7 }, new[] { -1 })]
    [InlineData(new[] { 5, 5, 5 }, new[] { -1, -1, -1 })]
    public void Compute_KnownInputs_MatchExpected(int[] input, int[] expected)
    {
        Assert.Equal(expected, NextSmallerElement.Compute(input));
    }

    [Fact]
    public void Compute_LargeRandom_MatchesNaive()
    {
        var rng = new Random(42);
        var arr = new int[10_000];
        for (var i = 0; i < arr.Length; i++)
        {
            arr[i] = rng.Next(-500, 500);
        }

        Assert.Equal(Naive(arr), NextSmallerElement.Compute(arr));
    }

    private static int[] Naive(int[] a)
    {
        var r = new int[a.Length];
        for (var i = 0; i < a.Length; i++)
        {
            r[i] = -1;
            for (var j = i + 1; j < a.Length; j++)
            {
                if (a[j] < a[i])
                {
                    r[i] = a[j];
                    break;
                }
            }
        }

        return r;
    }
}
