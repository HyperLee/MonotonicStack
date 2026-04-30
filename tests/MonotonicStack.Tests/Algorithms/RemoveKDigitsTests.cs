using MonotonicStack.Algorithms;

namespace MonotonicStack.Tests.Algorithms;

public class RemoveKDigitsTests
{
    [Theory]
    [InlineData("1432219", 3, "1219")]
    [InlineData("10200", 1, "200")]
    [InlineData("10", 2, "0")]
    [InlineData("112", 1, "11")]
    [InlineData("1234567890", 9, "0")]
    [InlineData("9", 1, "0")]
    [InlineData("100", 1, "0")]
    [InlineData("123456", 0, "123456")]
    [InlineData("9876543210", 5, "43210")]
    public void Compute_KnownInputs_MatchExpected(string num, int k, string expected)
    {
        Assert.Equal(expected, RemoveKDigits.Compute(num, k));
    }

    [Fact]
    public void Compute_NullInput_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => RemoveKDigits.Compute(null!, 1));
    }

    [Fact]
    public void Compute_NonDigitInput_Throws()
    {
        Assert.Throws<ArgumentException>(() => RemoveKDigits.Compute("12a3", 1));
    }

    [Fact]
    public void Compute_NegativeK_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => RemoveKDigits.Compute("123", -1));
    }

    [Fact]
    public void Compute_KGreaterThanLength_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => RemoveKDigits.Compute("123", 4));
    }

    [Fact]
    public void Compute_EmptyInput_KZero_ReturnsZero()
    {
        Assert.Equal("0", RemoveKDigits.Compute(string.Empty, 0));
    }
}
