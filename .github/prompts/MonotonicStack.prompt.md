# Monotonic Stack 開發規格書

> 本文件為「單調棧（Monotonic Stack）」函式庫之開發規格書，適用於 .NET 10 / C# 14。
> 涵蓋背景知識、需求範圍、API 設計、專案結構、實作演算法、測試計畫與驗收標準。

---

## 1. 專案概述

### 1.1 背景知識

**單調棧（Monotonic Stack）** 是一種特殊用法的堆疊資料結構，棧內元素由棧底到棧頂始終維持「單調遞增」或「單調遞減」的順序。當新元素準備入棧時，會不斷彈出（Pop）破壞單調性的棧頂元素，直到滿足單調條件後，再將新元素壓入棧中。

其核心優勢是能在 **O(n) 攤銷時間複雜度** 內解決一類「尋找下一個（或上一個）更大／更小元素」的問題——每個元素至多入棧一次、出棧一次。

| 棧的型態 | 棧內順序（底→頂） | 典型用途 |
|---|---|---|
| 單調遞增棧 | 由小到大 | 找尋「左／右側第一個比自己小」的元素 |
| 單調遞減棧 | 由大到小 | 找尋「左／右側第一個比自己大」的元素 |

### 1.2 運作邏輯（以單調遞增棧為例）

1. 由左至右遍歷陣列中的每個元素。
2. 若當前元素 **小於或等於** 棧頂元素，則彈出棧頂（因該棧頂已不再是後續元素的「下一個更小」候選人）。
3. 重覆步驟 2，直到當前元素大於棧頂元素，或棧為空。
4. 將當前元素（通常是其索引）壓入棧中。
5. 遍歷結束後，棧內仍存留的元素表示其右側不存在比它更小的元素。

### 1.3 專案目標

提供一個高品質、易於重用、可測試的 .NET 類別庫，包含：

- 通用的 `MonotonicStack<T>` 泛型資料結構（支援 `IComparer<T>` 自訂比較）。
- 一組基於單調棧解決經典演算法問題的靜態工具類別。
- 一個示範用 Console 應用程式，展示各場景的輸入／輸出。
- 完整的 xUnit 單元測試，覆蓋正常案例、邊界案例與例外情境。

---

## 2. 範圍與交付物

### 2.1 交付項目

| 編號 | 交付物 | 專案類型 | 必要性 |
|---|---|---|---|
| D1 | `MonotonicStack` 類別庫 | `Microsoft.NET.Sdk` (Library, net10.0) | 必要 |
| D2 | `MonotonicStack.Demo` Console 範例 | `Microsoft.NET.Sdk` (Exe, net10.0) | 必要 |
| D3 | `MonotonicStack.Tests` 單元測試 | xUnit, net10.0 | 必要 |
| D4 | `MonotonicStack.sln` 方案檔（已存在，需新增專案參考） | — | 必要 |
| D5 | XML 文件註解（產出 `.xml` 文件檔） | 套用於 D1 公開 API | 必要 |

### 2.2 不在本次範圍

- 效能基準測試（BenchmarkDotNet）。
- 多執行緒／執行緒安全保證（本資料結構為非執行緒安全，需在文件中明示）。
- NuGet 套件發佈流程。
- 圖形化介面或 Web API。

---

## 3. 專案結構

```
MonotonicStack/
├── MonotonicStack.sln
├── src/
│   ├── MonotonicStack/                       # D1 類別庫
│   │   ├── MonotonicStack.csproj
│   │   ├── MonotonicStack.cs                 # 泛型核心資料結構
│   │   ├── MonotonicOrder.cs                 # 列舉：Increasing / Decreasing / NonDecreasing / NonIncreasing
│   │   ├── Algorithms/
│   │   │   ├── NextGreaterElement.cs
│   │   │   ├── NextSmallerElement.cs
│   │   │   ├── PreviousGreaterElement.cs
│   │   │   ├── PreviousSmallerElement.cs
│   │   │   ├── DailyTemperatures.cs
│   │   │   ├── StockSpan.cs
│   │   │   ├── TrappingRainWater.cs
│   │   │   ├── LargestRectangleInHistogram.cs
│   │   │   ├── RemoveKDigits.cs
│   │   │   └── SlidingWindowMaximum.cs
│   └── MonotonicStack.Demo/                  # D2 Console 範例
│       ├── MonotonicStack.Demo.csproj
│       └── Program.cs
└── tests/
    └── MonotonicStack.Tests/                 # D3 單元測試
        ├── MonotonicStack.Tests.csproj
        ├── MonotonicStackTests.cs
        └── Algorithms/
            ├── NextGreaterElementTests.cs
            ├── NextSmallerElementTests.cs
            ├── PreviousGreaterElementTests.cs
            ├── PreviousSmallerElementTests.cs
            ├── DailyTemperaturesTests.cs
            ├── StockSpanTests.cs
            ├── TrappingRainWaterTests.cs
            ├── LargestRectangleInHistogramTests.cs
            ├── RemoveKDigitsTests.cs
            └── SlidingWindowMaximumTests.cs
```

> 註：目前 repo 中已存在 `MonotonicStack/` 子資料夾（內含 `Program.cs`），重構時應將其改置於 `src/MonotonicStack.Demo/` 或就地調整 csproj。建議統一改採 `src/` + `tests/` 兩層結構並更新 `.sln`。

---

## 4. 核心 API 設計

### 4.1 `MonotonicOrder` 列舉

```csharp
namespace MonotonicStack;

/// <summary>
/// 描述單調棧的單調性方向。
/// </summary>
public enum MonotonicOrder
{
    /// <summary>嚴格遞增（由棧底到棧頂）。彈出條件：top &gt;= incoming。</summary>
    Increasing,
    /// <summary>嚴格遞減（由棧底到棧頂）。彈出條件：top &lt;= incoming。</summary>
    Decreasing,
    /// <summary>非遞減（允許相等）。彈出條件：top &gt; incoming。</summary>
    NonDecreasing,
    /// <summary>非遞增（允許相等）。彈出條件：top &lt; incoming。</summary>
    NonIncreasing,
}
```

### 4.2 `MonotonicStack<T>` 泛型核心類別

#### 4.2.1 型別宣告與建構子

```csharp
public sealed class MonotonicStack<T>
{
    public MonotonicStack(MonotonicOrder order);
    public MonotonicStack(MonotonicOrder order, IComparer<T> comparer);
    public MonotonicStack(MonotonicOrder order, IComparer<T> comparer, int capacity);
}
```

- 若呼叫者未提供 `IComparer<T>`，則使用 `Comparer<T>.Default`；當 `T` 不實作 `IComparable<T>` 時，於建構時拋出 `ArgumentException`。

#### 4.2.2 公開屬性

| 成員 | 型別 | 說明 |
|---|---|---|
| `Count` | `int` | 棧中目前元素數量。 |
| `IsEmpty` | `bool` | 棧是否為空。 |
| `Order` | `MonotonicOrder` | 此棧的單調性設定。 |

#### 4.2.3 公開方法

| 方法 | 簽章 | 說明 |
|---|---|---|
| `Push` | `IReadOnlyList<T> Push(T item)` | 壓入新元素，並回傳本次操作中被彈出的元素清單（按彈出順序，可能為空集合）。呼叫者可藉此追蹤「正被新元素覆蓋」的舊棧頂。 |
| `Peek` | `T Peek()` | 取得棧頂元素，若棧為空則拋出 `InvalidOperationException`。 |
| `TryPeek` | `bool TryPeek(out T item)` | 嘗試取得棧頂；空棧時回傳 `false`。 |
| `Pop` | `T Pop()` | 直接彈出棧頂（非單調操作），空棧時拋出 `InvalidOperationException`。 |
| `Clear` | `void Clear()` | 清空棧。 |
| `ToArray` | `T[] ToArray()` | 由棧底到棧頂輸出快照陣列。 |
| `GetEnumerator` | 實作 `IEnumerable<T>` | 由棧底到棧頂列舉。 |

#### 4.2.4 設計準則

- 內部以 `List<T>` 作為儲存體（提供 O(1) 末端 push/pop，且支援 capacity 預配置）。
- `MonotonicStack<T>` **非執行緒安全**，須於 XML 文件註解中明示。
- 所有公開 API 須附 XML doc，含 `<example>` 範例。

### 4.3 演算法靜態工具類別

每個經典場景對應一個靜態類別，所有方法皆為 pure function（不修改輸入）。輸入皆採 `ReadOnlySpan<T>` 或 `IReadOnlyList<T>`，回傳新陣列／集合。

| 類別 | 主要方法 | 簽章草案 |
|---|---|---|
| `NextGreaterElement` | `Compute` | `static int[] Compute(ReadOnlySpan<int> nums)`，回傳長度與 `nums` 相同；無解的位置填 `-1`。 |
| `NextSmallerElement` | `Compute` | 同上，無解填 `-1`。 |
| `PreviousGreaterElement` | `Compute` | 同上。 |
| `PreviousSmallerElement` | `Compute` | 同上。 |
| `DailyTemperatures` | `Compute` | `static int[] Compute(ReadOnlySpan<int> temperatures)`，回傳「需等待幾天」（無更高溫度填 `0`）。對應 LeetCode 739。 |
| `StockSpan` | `Compute` | `static int[] Compute(ReadOnlySpan<int> prices)`，計算每日股價跨度。對應 LeetCode 901。亦可提供 `StockSpanner` 線上類別（每呼叫 `Next(price)` 回傳當日跨度）。 |
| `TrappingRainWater` | `Compute` | `static int Compute(ReadOnlySpan<int> heights)`，回傳可接雨水總量。對應 LeetCode 42。 |
| `LargestRectangleInHistogram` | `Compute` | `static int Compute(ReadOnlySpan<int> heights)`，回傳最大矩形面積。對應 LeetCode 84。 |
| `RemoveKDigits` | `Compute` | `static string Compute(string num, int k)`，移除 `k` 位後使結果最小。對應 LeetCode 402。輸入需驗證：`num` 僅含 `0-9`、`k >= 0`、`k <= num.Length`；結果需去除前導零，全刪則回傳 `"0"`。 |
| `SlidingWindowMaximum` | `Compute` | `static int[] Compute(ReadOnlySpan<int> nums, int k)`，視窗大小為 `k` 的最大值序列。對應 LeetCode 239。本題使用單調遞減**雙端佇列（deque）**，需在 XML doc 中註明與單調棧的關聯與差異。 |

> **泛型擴充（可選）**：對 `NextGreater/NextSmaller/PreviousGreater/PreviousSmaller`，提供額外的泛型多載 `Compute<T>(ReadOnlySpan<T> source, IComparer<T>? comparer = null)`，回傳 `T?[]`（reference 型別填 `null`、value 型別包成 `Nullable<T>`）。實作上以索引版本為核心，再投影回值。

### 4.4 例外與錯誤處理

| 情境 | 行為 |
|---|---|
| `Peek` / `Pop` 於空棧 | 拋出 `InvalidOperationException`，訊息：「Stack is empty.」 |
| `T` 無 `IComparable<T>` 且未提供 `IComparer<T>` | 於建構子拋出 `ArgumentException`。 |
| `comparer` 為 `null`（除允許省略的多載外） | 拋出 `ArgumentNullException(nameof(comparer))`。 |
| `RemoveKDigits` 之 `num` 為 `null` | 拋出 `ArgumentNullException`。 |
| `RemoveKDigits` 之 `num` 含非數字字元 | 拋出 `ArgumentException`。 |
| `RemoveKDigits` 之 `k < 0` 或 `k > num.Length` | 拋出 `ArgumentOutOfRangeException`。 |
| `SlidingWindowMaximum` 之 `k <= 0` 或 `k > nums.Length` | 拋出 `ArgumentOutOfRangeException`。 |

---

## 5. 演算法設計（虛擬碼）

### 5.1 `MonotonicStack<T>.Push`

```text
poppedItems = new List<T>()
while stack not empty AND violatesOrder(stack.top, item):
    poppedItems.Add(stack.PopInternal())
stack.PushInternal(item)
return poppedItems

violatesOrder 依 Order 而定：
    Increasing     : top >= item
    Decreasing     : top <= item
    NonDecreasing  : top >  item
    NonIncreasing  : top <  item
```

### 5.2 Next Greater Element（範例）

```text
result = new int[n] filled with -1
stack = empty (儲存「尚未找到下一個更大值」的索引)
for i from 0 to n-1:
    while stack not empty AND nums[stack.top] < nums[i]:
        idx = stack.Pop()
        result[idx] = nums[i]
    stack.Push(i)
return result
```

### 5.3 Trapping Rain Water（單調遞減棧）

```text
total = 0
stack = empty (索引)
for i from 0 to n-1:
    while stack not empty AND heights[stack.top] < heights[i]:
        bottom = stack.Pop()
        if stack is empty: break
        left = stack.top
        width = i - left - 1
        boundedHeight = min(heights[left], heights[i]) - heights[bottom]
        total += width * boundedHeight
    stack.Push(i)
return total
```

### 5.4 Largest Rectangle in Histogram

```text
向 heights 末尾追加哨兵 0 簡化邏輯。
stack = empty (索引；維持 heights 嚴格遞增)
maxArea = 0
for i from 0 to n (含哨兵):
    h = (i == n ? 0 : heights[i])
    while stack not empty AND heights[stack.top] > h:
        top = stack.Pop()
        left = stack is empty ? -1 : stack.top
        width = i - left - 1
        maxArea = max(maxArea, heights[top] * width)
    stack.Push(i)
return maxArea
```

### 5.5 Remove K Digits

```text
stack = empty (char)
for ch in num:
    while k > 0 AND stack not empty AND stack.top > ch:
        stack.Pop(); k--
    stack.Push(ch)
while k > 0: stack.Pop(); k--   // 處理單調不減的尾部
result = new string(stack 由底到頂)
trim leading zeros
return result == "" ? "0" : result
```

### 5.6 Sliding Window Maximum（單調遞減 deque）

```text
deque = empty (索引；對應值嚴格遞減)
result = new int[n - k + 1]
for i from 0 to n-1:
    while deque not empty AND deque.front <= i - k:
        deque.PopFront()
    while deque not empty AND nums[deque.back] <= nums[i]:
        deque.PopBack()
    deque.PushBack(i)
    if i >= k - 1:
        result[i - k + 1] = nums[deque.front]
return result
```

---

## 6. Console 範例（D2）

`MonotonicStack.Demo/Program.cs` 應提供互動式選單，列出所有經典場景，逐一執行內建範例輸入並印出結果。建議採用 `switch` 表達式分派：

```
請選擇要展示的場景：
 1) Next Greater Element        例：[2,1,2,4,3,1] → [4,2,4,-1,-1,-1]
 2) Next Smaller Element
 3) Previous Greater Element
 4) Previous Smaller Element
 5) Daily Temperatures          例：[73,74,75,71,69,72,76,73] → [1,1,4,2,1,1,0,0]
 6) Stock Span                  例：[100,80,60,70,60,75,85] → [1,1,1,2,1,4,6]
 7) Trapping Rain Water         例：[0,1,0,2,1,0,1,3,2,1,2,1] → 6
 8) Largest Rectangle Histogram 例：[2,1,5,6,2,3] → 10
 9) Remove K Digits             例：("1432219", 3) → "1219"
10) Sliding Window Maximum      例：([1,3,-1,-3,5,3,6,7], 3) → [3,3,5,5,6,7]
 0) 離開
```

每個場景至少展示一組固定 sample，並接受使用者自由輸入（可選）。

---

## 7. 測試計畫（D3）

### 7.1 框架與套件

- xUnit (`xunit`, `xunit.runner.visualstudio`)。
- `Microsoft.NET.Test.Sdk`。
- `coverlet.collector`（程式碼覆蓋率）。

### 7.2 測試覆蓋規範

- 公開 API 程式碼覆蓋率（line + branch）目標 ≥ **90%**。
- 每個演算法類別至少包含：
  1. **空輸入**（empty span / empty string）。
  2. **單一元素**。
  3. **單調遞增 / 單調遞減** 序列。
  4. **全部相等** 序列。
  5. **包含負數**（適用時）。
  6. **LeetCode 官方提供之最少 2 組範例**。
  7. **大型隨機輸入**（n = 10,000）與「樸素 O(n²) 實作」的結果交叉驗證（property-based 風格）。
  8. **例外案例**（null、out-of-range、格式錯誤等）。
- `MonotonicStack<T>` 須額外覆蓋：
  - 四種 `MonotonicOrder` 各自的入棧／彈出行為。
  - `Push` 回傳值正確反映被彈出的元素。
  - 自訂 `IComparer<T>`（例如反向比較、字串長度比較）。
  - `Peek/Pop` 於空棧拋例外。

### 7.3 測試命名

採 `MethodName_Scenario_ExpectedBehavior` 風格，例如：

```csharp
[Fact]
public void Compute_EmptyInput_ReturnsEmptyArray() { ... }

[Theory]
[InlineData(new[] { 2, 1, 2, 4, 3, 1 }, new[] { 4, 2, 4, -1, -1, -1 })]
public void Compute_LeetCodeSamples_ReturnsExpected(int[] input, int[] expected) { ... }
```

不於測試方法內加註 `// Arrange / // Act / // Assert` 註解（依 C# instructions）。

---

## 8. 編碼規範（沿用 `.github/instructions/csharp.instructions.md`）

- 使用 C# 14、`net10.0`，`<Nullable>enable</Nullable>`、`<ImplicitUsings>enable</ImplicitUsings>`。
- File-scoped namespace；使用 `is null` / `is not null`。
- 公開 API 一律附 XML doc（含 `<summary>`、`<param>`、`<returns>`、`<exception>`、`<example>`）。
- 私有欄位採 camelCase；公開成員採 PascalCase；介面以 `I` 為前綴。
- 善用 pattern matching 與 switch expression。
- 字串／成員引用使用 `nameof`。
- 啟用 `<GenerateDocumentationFile>true</GenerateDocumentationFile>` 並將 `CS1591` 視為警告而非錯誤（測試與 Demo 專案可關閉）。
- 啟用 `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>`（建議於 Library 專案）。

---

## 9. 開發步驟（建議順序）

1. 建立 `src/` 與 `tests/` 結構；新增 `MonotonicStack` Library、`MonotonicStack.Demo` Console、`MonotonicStack.Tests` xUnit 專案；更新 `.sln`。
2. 實作 `MonotonicOrder` 列舉與 `MonotonicStack<T>` 核心類別 + 對應單元測試。
3. 依下列順序逐一實作演算法（每個演算法皆採 TDD：先寫測試 → 實作 → 重構）：
   1. NextGreaterElement / NextSmallerElement / PreviousGreaterElement / PreviousSmallerElement
   2. DailyTemperatures
   3. StockSpan（含線上版 `StockSpanner`）
   4. TrappingRainWater
   5. LargestRectangleInHistogram
   6. RemoveKDigits
   7. SlidingWindowMaximum
4. 串接 `MonotonicStack.Demo` 互動選單。
5. 補完 XML 文件、README（可選）、覆蓋率檢查。
6. `dotnet build` 與 `dotnet test` 全綠。

---

## 10. 驗收標準（Definition of Done）

- [ ] `dotnet build` 在 `Release` 設定下零警告、零錯誤。
- [ ] `dotnet test` 全部通過，且覆蓋率報告顯示 Library 專案 line coverage ≥ 90%。
- [ ] 所有公開型別與成員皆含 XML 文件註解。
- [ ] Console Demo 可執行，並能正確展示所有 10 個經典場景的範例輸出。
- [ ] 所有演算法之時間複雜度為 O(n)（`SlidingWindowMaximum` 為 O(n)；`RemoveKDigits` 為 O(n)）；空間複雜度為 O(n)。
- [ ] 程式碼通過 `.editorconfig` 之格式化檢查。
- [ ] 規格書中列出的所有例外情境皆有對應測試。

---

## 11. 參考資料

- LeetCode 經典題目：496（Next Greater Element I）、503、739（Daily Temperatures）、901（Online Stock Span）、42（Trapping Rain Water）、84（Largest Rectangle in Histogram）、402（Remove K Digits）、239（Sliding Window Maximum）。
- 演算法導論（CLRS）— Stack 章節。
- C# 開發規範：`.github/instructions/csharp.instructions.md`。
