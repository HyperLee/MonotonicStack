using MonotonicStack.Algorithms;

namespace MonotonicStack.Tests.Algorithms;

public class NextGreaterElementTests
{
    [Fact]
    public void Compute_EmptyInput_ReturnsEmptyArray()
    {
        Assert.Empty(NextGreaterElement.Compute(ReadOnlySpan<int>.Empty));
    }

    [Fact]
    public void Compute_SingleElement_ReturnsMinusOne()
    {
        Assert.Equal(new[] { -1 }, NextGreaterElement.Compute(new[] { 5 }));
    }

    [Theory]
    [InlineData(new[] { 2, 1, 2, 4, 3, 1 }, new[] { 4, 2, 4, -1, -1, -1 })]
    [InlineData(new[] { 1, 2, 3, 4 }, new[] { 2, 3, 4, -1 })]
    [InlineData(new[] { 4, 3, 2, 1 }, new[] { -1, -1, -1, -1 })]
    [InlineData(new[] { 5, 5, 5 }, new[] { -1, -1, -1 })]
    [InlineData(new[] { -1, -3, 2, -2 }, new[] { 2, 2, -1, -1 })]
    public void Compute_KnownInputs_MatchExpected(int[] input, int[] expected)
    {
        Assert.Equal(expected, NextGreaterElement.Compute(input));
    }

    [Fact]
    public void Compute_LargeRandom_MatchesNaive()
    {
        var rng = new Random(1234);
        var n = 10_000;
        var arr = new int[n];
        for (var i = 0; i < n; i++)
        {
            arr[i] = rng.Next(-1000, 1000);
        }

        var actual = NextGreaterElement.Compute(arr);
        var expected = NaiveNextGreater(arr);
        Assert.Equal(expected, actual);
    }

    private static int[] NaiveNextGreater(int[] a)
    {
        var r = new int[a.Length];
        for (var i = 0; i < a.Length; i++)
        {
            r[i] = -1;
            for (var j = i + 1; j < a.Length; j++)
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
