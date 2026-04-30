namespace MonotonicStack.Algorithms;

/// <summary>
/// LeetCode 239 - Sliding Window Maximum。利用單調遞減 deque（雙端佇列），時間複雜度 O(n)。
/// </summary>
/// <remarks>
/// 此問題雖屬「單調」家族，但需要從佇列前端淘汰過期索引，故採用 deque 而非單向 stack。
/// </remarks>
public static class SlidingWindowMaximum
{
    /// <summary>
    /// 計算每個大小為 <paramref name="k"/> 的視窗中的最大值序列。
    /// </summary>
    /// <param name="nums">輸入序列。</param>
    /// <param name="k">視窗大小。</param>
    /// <returns>長度為 <c>nums.Length - k + 1</c> 的最大值序列。</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="k"/> 不在 <c>1..nums.Length</c> 範圍。</exception>
    /// <example>
    /// <code>
    /// SlidingWindowMaximum.Compute(new[] { 1, 3, -1, -3, 5, 3, 6, 7 }, 3);
    /// // → [3, 3, 5, 5, 6, 7]
    /// </code>
    /// </example>
    public static int[] Compute(ReadOnlySpan<int> nums, int k)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(k, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(k, nums.Length);

        var n = nums.Length;
        var result = new int[n - k + 1];
        var deque = new LinkedList<int>();
        for (var i = 0; i < n; i++)
        {
            while (deque.Count > 0 && deque.First!.Value <= i - k)
            {
                deque.RemoveFirst();
            }

            while (deque.Count > 0 && nums[deque.Last!.Value] <= nums[i])
            {
                deque.RemoveLast();
            }

            deque.AddLast(i);
            if (i >= k - 1)
            {
                result[i - k + 1] = nums[deque.First!.Value];
            }
        }

        return result;
    }
}
