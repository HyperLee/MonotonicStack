using System.Text;

namespace MonotonicStack.Algorithms;

/// <summary>
/// LeetCode 402 - Remove K Digits。時間複雜度 O(n)。
/// </summary>
public static class RemoveKDigits
{
    /// <summary>
    /// 從 <paramref name="num"/> 中移除 <paramref name="k"/> 位後使數值最小的字串。
    /// </summary>
    /// <param name="num">由 <c>0-9</c> 組成的字串。</param>
    /// <param name="k">欲移除的位數。</param>
    /// <returns>移除後的最小數值字串；全部移除時回傳 <c>"0"</c>。</returns>
    /// <exception cref="ArgumentNullException"><paramref name="num"/> 為 <see langword="null"/>。</exception>
    /// <exception cref="ArgumentException"><paramref name="num"/> 含非數字字元。</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="k"/> 為負或大於字串長度。</exception>
    /// <example>
    /// <code>
    /// RemoveKDigits.Compute("1432219", 3); // "1219"
    /// RemoveKDigits.Compute("10200", 1);   // "200"
    /// RemoveKDigits.Compute("10", 2);      // "0"
    /// </code>
    /// </example>
    public static string Compute(string num, int k)
    {
        ArgumentNullException.ThrowIfNull(num);
        ArgumentOutOfRangeException.ThrowIfNegative(k);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(k, num.Length);

        for (var i = 0; i < num.Length; i++)
        {
            if (num[i] < '0' || num[i] > '9')
            {
                throw new ArgumentException("Input must contain only digits 0-9.", nameof(num));
            }
        }

        if (k == num.Length)
        {
            return "0";
        }

        var stack = new List<char>(num.Length);
        var remaining = k;
        foreach (var ch in num)
        {
            while (remaining > 0 && stack.Count > 0 && stack[^1] > ch)
            {
                stack.RemoveAt(stack.Count - 1);
                remaining--;
            }

            stack.Add(ch);
        }

        if (remaining > 0)
        {
            stack.RemoveRange(stack.Count - remaining, remaining);
        }

        var start = 0;
        while (start < stack.Count && stack[start] == '0')
        {
            start++;
        }

        if (start >= stack.Count)
        {
            return "0";
        }

        var sb = new StringBuilder(stack.Count - start);
        for (var i = start; i < stack.Count; i++)
        {
            sb.Append(stack[i]);
        }

        return sb.ToString();
    }
}
