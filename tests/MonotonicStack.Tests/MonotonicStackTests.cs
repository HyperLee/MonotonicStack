using System.Collections.Generic;

namespace MonotonicStack.Tests;

public class MonotonicStackTests
{
    [Fact]
    public void NewStack_IsEmpty()
    {
        var s = new MonotonicStack<int>(MonotonicOrder.Increasing);
        Assert.True(s.IsEmpty);
        Assert.Equal(0, s.Count);
        Assert.Equal(MonotonicOrder.Increasing, s.Order);
    }

    [Fact]
    public void Push_Increasing_PopsViolatingTops()
    {
        var s = new MonotonicStack<int>(MonotonicOrder.Increasing);
        Assert.Empty(s.Push(1));
        Assert.Empty(s.Push(3));
        var popped = s.Push(2);
        Assert.Equal(new[] { 3 }, popped);
        Assert.Equal(new[] { 1, 2 }, s.ToArray());
    }

    [Fact]
    public void Push_Increasing_EqualValuePopsTop()
    {
        var s = new MonotonicStack<int>(MonotonicOrder.Increasing);
        s.Push(1);
        s.Push(2);
        var popped = s.Push(2);
        Assert.Equal(new[] { 2 }, popped);
        Assert.Equal(new[] { 1, 2 }, s.ToArray());
    }

    [Fact]
    public void Push_NonDecreasing_AllowsEqualValues()
    {
        var s = new MonotonicStack<int>(MonotonicOrder.NonDecreasing);
        s.Push(1);
        s.Push(2);
        var popped = s.Push(2);
        Assert.Empty(popped);
        Assert.Equal(new[] { 1, 2, 2 }, s.ToArray());
    }

    [Fact]
    public void Push_Decreasing_PopsViolatingTops()
    {
        var s = new MonotonicStack<int>(MonotonicOrder.Decreasing);
        s.Push(5);
        s.Push(3);
        var popped = s.Push(4);
        Assert.Equal(new[] { 3 }, popped);
        Assert.Equal(new[] { 5, 4 }, s.ToArray());
    }

    [Fact]
    public void Push_NonIncreasing_AllowsEqualValues()
    {
        var s = new MonotonicStack<int>(MonotonicOrder.NonIncreasing);
        s.Push(5);
        s.Push(5);
        Assert.Empty(s.Push(5));
        Assert.Equal(new[] { 5, 5, 5 }, s.ToArray());
    }

    [Fact]
    public void Peek_EmptyStack_Throws()
    {
        var s = new MonotonicStack<int>(MonotonicOrder.Increasing);
        Assert.Throws<InvalidOperationException>(() => s.Peek());
    }

    [Fact]
    public void Pop_EmptyStack_Throws()
    {
        var s = new MonotonicStack<int>(MonotonicOrder.Increasing);
        Assert.Throws<InvalidOperationException>(() => s.Pop());
    }

    [Fact]
    public void TryPeek_EmptyAndNonEmpty_Works()
    {
        var s = new MonotonicStack<int>(MonotonicOrder.Increasing);
        Assert.False(s.TryPeek(out _));
        s.Push(7);
        Assert.True(s.TryPeek(out var v));
        Assert.Equal(7, v);
    }

    [Fact]
    public void Pop_RemovesAndReturnsTop()
    {
        var s = new MonotonicStack<int>(MonotonicOrder.Increasing);
        s.Push(1);
        s.Push(2);
        Assert.Equal(2, s.Pop());
        Assert.Equal(1, s.Peek());
    }

    [Fact]
    public void Clear_RemovesAllElements()
    {
        var s = new MonotonicStack<int>(MonotonicOrder.Increasing);
        s.Push(1);
        s.Push(2);
        s.Clear();
        Assert.True(s.IsEmpty);
    }

    [Fact]
    public void Enumerator_IteratesBottomToTop()
    {
        var s = new MonotonicStack<int>(MonotonicOrder.Increasing);
        s.Push(1);
        s.Push(2);
        s.Push(3);
        Assert.Equal(new[] { 1, 2, 3 }, s);
    }

    [Fact]
    public void Constructor_NullComparer_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new MonotonicStack<int>(MonotonicOrder.Increasing, null!));
    }

    [Fact]
    public void Constructor_NegativeCapacity_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new MonotonicStack<int>(MonotonicOrder.Increasing, Comparer<int>.Default, -1));
    }

    [Fact]
    public void Constructor_TypeNotComparable_Throws()
    {
        Assert.Throws<ArgumentException>(() => new MonotonicStack<NotComparable>(MonotonicOrder.Increasing));
    }

    [Fact]
    public void CustomComparer_ReverseInt_Works()
    {
        // 反向比較器使「越大」越小，所以遞增棧實際上會維持原值的遞減。
        var reverse = Comparer<int>.Create((a, b) => b.CompareTo(a));
        var s = new MonotonicStack<int>(MonotonicOrder.Increasing, reverse);
        s.Push(1);
        var popped = s.Push(3);
        Assert.Equal(new[] { 1 }, popped);
        Assert.Equal(new[] { 3 }, s.ToArray());
    }

    [Fact]
    public void CustomComparer_StringByLength_Works()
    {
        var byLen = Comparer<string>.Create((a, b) => a.Length.CompareTo(b.Length));
        var s = new MonotonicStack<string>(MonotonicOrder.Increasing, byLen);
        s.Push("a");
        s.Push("bbb");
        var popped = s.Push("cc");
        Assert.Equal(new[] { "bbb" }, popped);
        Assert.Equal(new[] { "a", "cc" }, s.ToArray());
    }

    private sealed class NotComparable
    {
    }
}
