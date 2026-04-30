namespace MonotonicStack;

/// <summary>
/// 描述單調棧的單調性方向（由棧底到棧頂）。
/// </summary>
public enum MonotonicOrder
{
    /// <summary>嚴格遞增。彈出條件：top &gt;= incoming。</summary>
    Increasing,

    /// <summary>嚴格遞減。彈出條件：top &lt;= incoming。</summary>
    Decreasing,

    /// <summary>非遞減（允許相等）。彈出條件：top &gt; incoming。</summary>
    NonDecreasing,

    /// <summary>非遞增（允許相等）。彈出條件：top &lt; incoming。</summary>
    NonIncreasing,
}
