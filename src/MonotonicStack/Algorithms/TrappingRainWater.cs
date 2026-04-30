namespace MonotonicStack.Algorithms;

/// <summary>
/// LeetCode 42 - Trapping Rain Water。時間複雜度 O(n)。
/// </summary>
public static class TrappingRainWater
{
    /// <summary>
    /// 計算 <paramref name="heights"/> 表示之地形可蓄水總量。
    /// </summary>
    /// <param name="heights">非負整數高度序列。</param>
    /// <returns>可蓄水總量。</returns>
    /// <exception cref="ArgumentOutOfRangeException">序列中含有負值高度。</exception>
    /// <example>
    /// <code>
    /// TrappingRainWater.Compute(new[] { 0, 1, 0, 2, 1, 0, 1, 3, 2, 1, 2, 1 }); // 6
    /// </code>
    /// </example>
    public static int Compute(ReadOnlySpan<int> heights)
    {
        for (var j = 0; j < heights.Length; j++)
        {
            if (heights[j] < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(heights), "Heights must be non-negative.");
            }
        }

        var total = 0;
        var stack = new Stack<int>(heights.Length);
        for (var i = 0; i < heights.Length; i++)
        {
            while (stack.Count > 0 && heights[stack.Peek()] < heights[i])
            {
                var bottom = stack.Pop();
                if (stack.Count == 0)
                {
                    break;
                }

                var left = stack.Peek();
                var width = i - left - 1;
                var bounded = Math.Min(heights[left], heights[i]) - heights[bottom];
                total += width * bounded;
            }

            stack.Push(i);
        }

        return total;
    }
}
