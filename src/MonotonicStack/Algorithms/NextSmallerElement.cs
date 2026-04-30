namespace MonotonicStack.Algorithms;

/// <summary>
/// 對輸入序列計算每個位置「右側第一個嚴格小於自己」的元素值。時間複雜度 O(n)。
/// </summary>
public static class NextSmallerElement
{
    /// <summary>
    /// 計算 <paramref name="nums"/> 中，每個位置右側第一個嚴格小於自己的元素值；
    /// 若不存在則填 <c>-1</c>。
    /// </summary>
    /// <param name="nums">輸入序列。</param>
    /// <returns>長度與 <paramref name="nums"/> 相同的結果陣列。</returns>
    public static int[] Compute(ReadOnlySpan<int> nums)
    {
        var n = nums.Length;
        var result = new int[n];
        result.AsSpan().Fill(-1);

        var stack = new Stack<int>(n);
        for (var i = 0; i < n; i++)
        {
            while (stack.Count > 0 && nums[stack.Peek()] > nums[i])
            {
                result[stack.Pop()] = nums[i];
            }

            stack.Push(i);
        }

        return result;
    }
}
