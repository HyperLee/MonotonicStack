using System.Collections;
using System.Collections.Generic;

namespace MonotonicStack;

/// <summary>
/// 通用的單調棧（Monotonic Stack）資料結構。棧內元素由棧底到棧頂，依
/// <see cref="MonotonicOrder"/> 維持指定的單調順序。
/// </summary>
/// <typeparam name="T">元素型別。若未提供 <see cref="IComparer{T}"/>，需實作 <see cref="IComparable{T}"/>。</typeparam>
/// <remarks>
/// 本資料結構 <b>非執行緒安全</b>。多執行緒情境請自行同步。
/// </remarks>
/// <example>
/// <code>
/// var stack = new MonotonicStack&lt;int&gt;(MonotonicOrder.Increasing);
/// stack.Push(1);
/// stack.Push(3);
/// var popped = stack.Push(2); // popped contains [3]
/// </code>
/// </example>
public sealed class MonotonicStack<T> : IEnumerable<T>
{
    private readonly List<T> items;
    private readonly IComparer<T> comparer;

    /// <summary>
    /// 使用指定的單調順序與預設比較器建立空棧。
    /// </summary>
    /// <param name="order">單調順序。</param>
    /// <exception cref="ArgumentException"><typeparamref name="T"/> 未實作 <see cref="IComparable{T}"/> 且未提供比較器。</exception>
    public MonotonicStack(MonotonicOrder order)
        : this(order, GetDefaultComparerOrThrow(), capacity: 0)
    {
    }

    /// <summary>
    /// 使用指定的單調順序與比較器建立空棧。
    /// </summary>
    /// <param name="order">單調順序。</param>
    /// <param name="comparer">元素比較器。</param>
    /// <exception cref="ArgumentNullException"><paramref name="comparer"/> 為 <see langword="null"/>。</exception>
    public MonotonicStack(MonotonicOrder order, IComparer<T> comparer)
        : this(order, comparer, capacity: 0)
    {
    }

    /// <summary>
    /// 使用指定的單調順序、比較器與初始容量建立空棧。
    /// </summary>
    /// <param name="order">單調順序。</param>
    /// <param name="comparer">元素比較器。</param>
    /// <param name="capacity">初始容量。</param>
    /// <exception cref="ArgumentNullException"><paramref name="comparer"/> 為 <see langword="null"/>。</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> 為負數。</exception>
    public MonotonicStack(MonotonicOrder order, IComparer<T> comparer, int capacity)
    {
        ArgumentNullException.ThrowIfNull(comparer);
        ArgumentOutOfRangeException.ThrowIfNegative(capacity);

        Order = order;
        this.comparer = comparer;
        this.items = new List<T>(capacity);
    }

    /// <summary>取得棧中目前元素數量。</summary>
    public int Count => this.items.Count;

    /// <summary>取得棧是否為空。</summary>
    public bool IsEmpty => this.items.Count == 0;

    /// <summary>取得此棧的單調性設定。</summary>
    public MonotonicOrder Order { get; }

    /// <summary>
    /// 壓入新元素，先彈出所有違反單調性的棧頂，再將新元素推入。
    /// </summary>
    /// <param name="item">欲壓入的元素。</param>
    /// <returns>本次壓入過程中被彈出的元素清單，依彈出順序排列；若無彈出則為空集合。</returns>
    public IReadOnlyList<T> Push(T item)
    {
        List<T>? popped = null;
        while (this.items.Count > 0 && Violates(this.items[^1], item))
        {
            popped ??= new List<T>();
            popped.Add(this.items[^1]);
            this.items.RemoveAt(this.items.Count - 1);
        }

        this.items.Add(item);
        return popped ?? (IReadOnlyList<T>)Array.Empty<T>();
    }

    /// <summary>
    /// 取得棧頂元素而不彈出。
    /// </summary>
    /// <returns>棧頂元素。</returns>
    /// <exception cref="InvalidOperationException">棧為空。</exception>
    public T Peek()
    {
        if (this.items.Count == 0)
        {
            throw new InvalidOperationException("Stack is empty.");
        }

        return this.items[^1];
    }

    /// <summary>
    /// 嘗試取得棧頂元素。
    /// </summary>
    /// <param name="item">當棧非空時，回傳棧頂元素；否則為預設值。</param>
    /// <returns>棧非空時為 <see langword="true"/>，否則為 <see langword="false"/>。</returns>
    public bool TryPeek(out T item)
    {
        if (this.items.Count == 0)
        {
            item = default!;
            return false;
        }

        item = this.items[^1];
        return true;
    }

    /// <summary>
    /// 直接彈出棧頂元素（非單調操作）。
    /// </summary>
    /// <returns>被彈出的棧頂元素。</returns>
    /// <exception cref="InvalidOperationException">棧為空。</exception>
    public T Pop()
    {
        if (this.items.Count == 0)
        {
            throw new InvalidOperationException("Stack is empty.");
        }

        var top = this.items[^1];
        this.items.RemoveAt(this.items.Count - 1);
        return top;
    }

    /// <summary>清空棧中的所有元素。</summary>
    public void Clear() => this.items.Clear();

    /// <summary>
    /// 由棧底到棧頂輸出快照陣列。
    /// </summary>
    /// <returns>包含目前所有元素的新陣列。</returns>
    public T[] ToArray() => this.items.ToArray();

    /// <inheritdoc />
    public IEnumerator<T> GetEnumerator() => this.items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)this.items).GetEnumerator();

    // 判斷棧頂元素 (top) 是否違反單調性，需要在 incoming 進入時被彈出。
    private bool Violates(T top, T incoming)
    {
        var cmp = this.comparer.Compare(top, incoming);
        return Order switch
        {
            MonotonicOrder.Increasing => cmp >= 0,
            MonotonicOrder.Decreasing => cmp <= 0,
            MonotonicOrder.NonDecreasing => cmp > 0,
            MonotonicOrder.NonIncreasing => cmp < 0,
            _ => throw new InvalidOperationException($"Unknown {nameof(MonotonicOrder)} value: {Order}."),
        };
    }

    // 取得 Comparer<T>.Default；若 T 不可比較，於建構子改丟 ArgumentException。
    private static IComparer<T> GetDefaultComparerOrThrow()
    {
        if (typeof(IComparable<T>).IsAssignableFrom(typeof(T)) ||
            typeof(IComparable).IsAssignableFrom(typeof(T)))
        {
            return Comparer<T>.Default;
        }

        throw new ArgumentException(
            $"Type '{typeof(T)}' does not implement {nameof(IComparable)} or IComparable<T>; please supply an explicit comparer.");
    }
}
