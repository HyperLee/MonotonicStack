using MonotonicStack.Algorithms;

namespace MonotonicStack.Tests.Algorithms;

public class DailyTemperaturesTests
{
    [Fact]
    public void Compute_EmptyInput_ReturnsEmptyArray()
    {
        Assert.Empty(DailyTemperatures.Compute(ReadOnlySpan<int>.Empty));
    }

    [Theory]
    [InlineData(new[] { 73, 74, 75, 71, 69, 72, 76, 73 }, new[] { 1, 1, 4, 2, 1, 1, 0, 0 })]
    [InlineData(new[] { 30, 40, 50, 60 }, new[] { 1, 1, 1, 0 })]
    [InlineData(new[] { 30, 60, 90 }, new[] { 1, 1, 0 })]
    [InlineData(new[] { 50 }, new[] { 0 })]
    [InlineData(new[] { 50, 50, 50 }, new[] { 0, 0, 0 })]
    public void Compute_KnownInputs_MatchExpected(int[] input, int[] expected)
    {
        Assert.Equal(expected, DailyTemperatures.Compute(input));
    }

    [Fact]
    public void Compute_LargeRandom_MatchesNaive()
    {
        var rng = new Random(5);
        var arr = new int[10_000];
        for (var i = 0; i < arr.Length; i++)
        {
            arr[i] = rng.Next(0, 100);
        }

        Assert.Equal(Naive(arr), DailyTemperatures.Compute(arr));
    }

    private static int[] Naive(int[] a)
    {
        var r = new int[a.Length];
        for (var i = 0; i < a.Length; i++)
        {
            for (var j = i + 1; j < a.Length; j++)
            {
                if (a[j] > a[i])
                {
                    r[i] = j - i;
                    break;
                }
            }
        }

        return r;
    }
}
