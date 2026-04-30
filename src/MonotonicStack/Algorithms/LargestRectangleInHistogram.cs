namespace MonotonicStack.Algorithms;

/// <summary>
/// LeetCode 84 - Largest Rectangle in Histogram。時間複雜度 O(n)。
/// </summary>
public static class LargestRectangleInHistogram
{
    /// <summary>
    /// 計算長條圖中可形成的最大矩形面積。
    /// </summary>
    /// <param name="heights">非負整數高度序列。</param>
    /// <returns>最大矩形面積。</returns>
    /// <exception cref="ArgumentOutOfRangeException">序列中含有負值高度。</exception>
    /// <example>
    /// <code>
    /// LargestRectangleInHistogram.Compute(new[] { 2, 1, 5, 6, 2, 3 }); // 10
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

        var n = heights.Length;
        var maxArea = 0;
        var stack = new Stack<int>(n + 1);

        for (var i = 0; i <= n; i++)
        {
            // 哨兵：在尾端視為高度 0，以便清空殘留棧內元素。
            var h = i == n ? 0 : heights[i];
            while (stack.Count > 0 && heights[stack.Peek()] > h)
            {
                var top = stack.Pop();
                var left = stack.Count == 0 ? -1 : stack.Peek();
                var width = i - left - 1;
                var area = heights[top] * width;
                if (area > maxArea)
                {
                    maxArea = area;
                }
            }

            stack.Push(i);
        }

        return maxArea;
    }
}
