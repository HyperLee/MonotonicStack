namespace MonotonicStack.Algorithms;

/// <summary>
/// 對輸入序列計算每個位置「右側第一個嚴格大於自己」的元素值。
/// 對應 LeetCode 496 / 503 概念。時間複雜度 O(n)。
/// </summary>
public static class NextGreaterElement
{
    /// <summary>
    /// 計算 <paramref name="nums"/> 中，每個位置右側第一個嚴格大於自己的元素值；
    /// 若不存在則填 <c>-1</c>。
    /// </summary>
    /// <param name="nums">輸入序列。</param>
    /// <returns>長度與 <paramref name="nums"/> 相同的結果陣列。</returns>
    /// <example>
    /// <code>
    /// NextGreaterElement.Compute(new[] { 2, 1, 2, 4, 3, 1 });
    /// // → [4, 2, 4, -1, -1, -1]
    /// </code>
    /// </example>
    public static int[] Compute(ReadOnlySpan<int> nums)
    {
        var n = nums.Length;
        var result = new int[n];
        result.AsSpan().Fill(-1);

        var stack = new Stack<int>(n);
        for (var i = 0; i < n; i++)
        {
            while (stack.Count > 0 && nums[stack.Peek()] < nums[i])
            {
                result[stack.Pop()] = nums[i];
            }

            stack.Push(i);
        }

        return result;
    }
}
