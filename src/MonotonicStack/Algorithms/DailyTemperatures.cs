namespace MonotonicStack.Algorithms;

/// <summary>
/// LeetCode 739 - Daily Temperatures。時間複雜度 O(n)。
/// </summary>
public static class DailyTemperatures
{
    /// <summary>
    /// 計算每日「需等待幾天才會出現更高氣溫」；之後沒有更高氣溫者填 <c>0</c>。
    /// </summary>
    /// <param name="temperatures">每日氣溫序列。</param>
    /// <returns>等待天數陣列。</returns>
    /// <example>
    /// <code>
    /// DailyTemperatures.Compute(new[] { 73, 74, 75, 71, 69, 72, 76, 73 });
    /// // → [1, 1, 4, 2, 1, 1, 0, 0]
    /// </code>
    /// </example>
    public static int[] Compute(ReadOnlySpan<int> temperatures)
    {
        var n = temperatures.Length;
        var result = new int[n];

        var stack = new Stack<int>(n);
        for (var i = 0; i < n; i++)
        {
            while (stack.Count > 0 && temperatures[stack.Peek()] < temperatures[i])
            {
                var idx = stack.Pop();
                result[idx] = i - idx;
            }

            stack.Push(i);
        }

        return result;
    }
}
