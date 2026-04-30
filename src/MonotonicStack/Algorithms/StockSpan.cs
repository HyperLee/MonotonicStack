namespace MonotonicStack.Algorithms;

/// <summary>
/// LeetCode 901 - Online Stock Span。時間複雜度 O(n) 攤銷。
/// </summary>
public static class StockSpan
{
    /// <summary>
    /// 一次性計算整段股價的 Span 序列。
    /// </summary>
    /// <param name="prices">每日股價序列。</param>
    /// <returns>每日股價跨度。</returns>
    /// <example>
    /// <code>
    /// StockSpan.Compute(new[] { 100, 80, 60, 70, 60, 75, 85 });
    /// // → [1, 1, 1, 2, 1, 4, 6]
    /// </code>
    /// </example>
    public static int[] Compute(ReadOnlySpan<int> prices)
    {
        var n = prices.Length;
        var result = new int[n];

        var stack = new Stack<(int Price, int Span)>(n);
        for (var i = 0; i < n; i++)
        {
            var span = 1;
            while (stack.Count > 0 && stack.Peek().Price <= prices[i])
            {
                span += stack.Pop().Span;
            }

            stack.Push((prices[i], span));
            result[i] = span;
        }

        return result;
    }
}

/// <summary>
/// LeetCode 901 - Online Stock Span 的線上版本。每呼叫 <see cref="Next(int)"/> 一次回傳當日 Span。
/// </summary>
public sealed class StockSpanner
{
    private readonly Stack<(int Price, int Span)> stack = new();

    /// <summary>
    /// 推入新一日的股價並回傳當日 Span。
    /// </summary>
    /// <param name="price">當日股價。</param>
    /// <returns>當日 Span。</returns>
    public int Next(int price)
    {
        var span = 1;
        while (this.stack.Count > 0 && this.stack.Peek().Price <= price)
        {
            span += this.stack.Pop().Span;
        }

        this.stack.Push((price, span));
        return span;
    }
}
