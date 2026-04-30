using MonotonicStack.Algorithms;

namespace MonotonicStack.Demo;

internal static class Program
{
    private static void Main()
    {
        while (true)
        {
            PrintMenu();
            Console.Write("> ");
            var input = Console.ReadLine();
            if (input is null)
            {
                return;
            }

            input = input.Trim();
            if (input == "0" || input.Equals("q", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Bye!");
                return;
            }

            try
            {
                Dispatch(input);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[error] {ex.Message}");
            }

            Console.WriteLine();
        }
    }

    private static void PrintMenu()
    {
        Console.WriteLine("=== Monotonic Stack Demo ===");
        Console.WriteLine(" 1) Next Greater Element        e.g. [2,1,2,4,3,1] -> [4,2,4,-1,-1,-1]");
        Console.WriteLine(" 2) Next Smaller Element");
        Console.WriteLine(" 3) Previous Greater Element");
        Console.WriteLine(" 4) Previous Smaller Element");
        Console.WriteLine(" 5) Daily Temperatures          e.g. [73,74,75,71,69,72,76,73] -> [1,1,4,2,1,1,0,0]");
        Console.WriteLine(" 6) Stock Span                  e.g. [100,80,60,70,60,75,85] -> [1,1,1,2,1,4,6]");
        Console.WriteLine(" 7) Trapping Rain Water         e.g. [0,1,0,2,1,0,1,3,2,1,2,1] -> 6");
        Console.WriteLine(" 8) Largest Rectangle Histogram e.g. [2,1,5,6,2,3] -> 10");
        Console.WriteLine(" 9) Remove K Digits             e.g. (\"1432219\", 3) -> \"1219\"");
        Console.WriteLine("10) Sliding Window Maximum      e.g. ([1,3,-1,-3,5,3,6,7], 3) -> [3,3,5,5,6,7]");
        Console.WriteLine(" 0) Quit");
    }

    private static void Dispatch(string choice)
    {
        switch (choice)
        {
            case "1":
            {
                var arr = new[] { 2, 1, 2, 4, 3, 1 };
                Show(arr, NextGreaterElement.Compute(arr));
                break;
            }
            case "2":
            {
                var arr = new[] { 2, 1, 2, 4, 3, 1 };
                Show(arr, NextSmallerElement.Compute(arr));
                break;
            }
            case "3":
            {
                var arr = new[] { 2, 1, 2, 4, 3, 1 };
                Show(arr, PreviousGreaterElement.Compute(arr));
                break;
            }
            case "4":
            {
                var arr = new[] { 2, 1, 2, 4, 3, 1 };
                Show(arr, PreviousSmallerElement.Compute(arr));
                break;
            }
            case "5":
            {
                var arr = new[] { 73, 74, 75, 71, 69, 72, 76, 73 };
                Show(arr, DailyTemperatures.Compute(arr));
                break;
            }
            case "6":
            {
                var arr = new[] { 100, 80, 60, 70, 60, 75, 85 };
                Show(arr, StockSpan.Compute(arr));
                break;
            }
            case "7":
            {
                var arr = new[] { 0, 1, 0, 2, 1, 0, 1, 3, 2, 1, 2, 1 };
                Console.WriteLine($"input  : [{string.Join(",", arr)}]");
                Console.WriteLine($"output : {TrappingRainWater.Compute(arr)}");
                break;
            }
            case "8":
            {
                var arr = new[] { 2, 1, 5, 6, 2, 3 };
                Console.WriteLine($"input  : [{string.Join(",", arr)}]");
                Console.WriteLine($"output : {LargestRectangleInHistogram.Compute(arr)}");
                break;
            }
            case "9":
            {
                const string num = "1432219";
                const int k = 3;
                Console.WriteLine($"input  : (\"{num}\", {k})");
                Console.WriteLine($"output : \"{RemoveKDigits.Compute(num, k)}\"");
                break;
            }
            case "10":
            {
                var arr = new[] { 1, 3, -1, -3, 5, 3, 6, 7 };
                const int k = 3;
                Console.WriteLine($"input  : [{string.Join(",", arr)}], k={k}");
                Console.WriteLine($"output : [{string.Join(",", SlidingWindowMaximum.Compute(arr, k))}]");
                break;
            }
            default:
                Console.WriteLine("Unknown option.");
                break;
        }
    }

    private static void Show(int[] input, int[] output)
    {
        Console.WriteLine($"input  : [{string.Join(",", input)}]");
        Console.WriteLine($"output : [{string.Join(",", output)}]");
    }
}
