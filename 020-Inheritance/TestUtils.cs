namespace CSharpLearning;

public static class TestUtils
{
    private static int _passCount;
    private static int _failCount;

    public static void Check(string id, bool condition)
    {
        if (condition)
        {
            _passCount++;
            Console.WriteLine($"{id} PASS");
        }
        else
        {
            _failCount++;
            Console.WriteLine($"{id} FAIL  <<<<<");
        }
    }

    // 保留旧名字，兼容 019 的写法；内部走 Check，一起计数
    public static void AssertTrue(bool condition, string description) => Check(description, condition);

    public static void CheckThrows<T>(string id, Action action) where T : Exception
    {
        try
        {
            action();
            Console.WriteLine($"    没有抛出异常，期望 {typeof(T).Name}");
            Check(id, false);
        }
        catch (T)
        {
            Check(id, true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"    实际抛出的是 {ex.GetType().Name}，期望 {typeof(T).Name}");
            Check(id, false);
        }
    }

    // 在 Program.cs 最后调用一次
    public static void PrintSummary()
    {
        Console.WriteLine();
        Console.WriteLine($"共 {_passCount + _failCount} 条：通过 {_passCount}，失败 {_failCount}");
    }
}