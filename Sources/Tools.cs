using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace testify
{
    public class Tools
    {
        private static Dictionary<ConsoleColor, string> __customPrefix = new Dictionary<ConsoleColor, string>()
        {
            [ConsoleColor.DarkGreen]  = "PASS",
            [ConsoleColor.DarkRed]    = "FAIL",
            [ConsoleColor.DarkBlue]   = "EXCL",
            [ConsoleColor.DarkYellow] = "NIMP"
        };

        public static void PrintTestResult(TestResult result)
        {
            for (var i = 0; i < result.Outputs.Count; i++)
                foreach ((string text, ConsoleColor color) in result.Outputs[i])
                    Trace(text, color);

            Trace("", default);
            Trace("> " + result.PassingCount + " Passing", ConsoleColor.Green);
            if (result.FailingCount > 0)
                Trace("> " + result.FailingCount + " Failing", ConsoleColor.Red);
            if (result.ExcludedCount > 0)
                Trace("> " + result.ExcludedCount + " Excluded", ConsoleColor.Blue);
            if (result.NotImplementedCount > 0)
                Trace("> " + result.NotImplementedCount + " NotImplemented", ConsoleColor.Yellow);
        }

        public static IEnumerable<T> GetEnumerableOfType<T>(params object[] constructorArgs) where T : class
        {
            List<T> objects = new List<T>();
            var subClasses = Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T)));
            foreach (Type type in subClasses)
                objects.Add((T)Activator.CreateInstance(type, constructorArgs));
            //objects.Sort();
            return objects;
        }

        public static async Task<int> RunTestAsync(ATestSuite test)
        {
            // first test has a performance hit probably related to injection of function and generation of temp object
            // running and clearing the first hides the performance hit.
            test.Run();
            test.Result = new TestResult();
            test.Run();
            await test.Task;
            Tools.PrintTestResult(test.Result);
            return test.ExitCode;
        }
        public static async Task<int> RunTestsAsync()
        {
            var tests = GetEnumerableOfType<ATestSuite>().ToList();
            
            // first test has a performance hit probably related to injection of function and generation of temp object
            // running and clearing the first hides the performance hit.
            tests[0].Run();
            tests[0].Result = new TestResult();
            
            tests.ForEach( t => t.Run());

            var tasks = tests.Select(t => t.Task);
            await Task.WhenAll(tasks);
            var result = new TestResult();
            foreach (var test in tests)
            {
                result.Outputs.AddRange(test.Outputs);
                result.FailingCount += test.Result.FailingCount;
                result.PassingCount += test.Result.PassingCount;
                result.ExcludedCount += test.Result.ExcludedCount;
                result.NotImplementedCount += test.Result.NotImplementedCount;
            }
            Tools.PrintTestResult(result);
            return tests.Any(t => t.ExitCode != 0) ? 1 : 0;
        }

        public static void Assert(object a, object b)
        {
            if (a.Equals(b) == false)
                throw new Exception("Assertion failed on " + JsonSerializer.Serialize(a) + " == " + JsonSerializer.Serialize(b));
        }

        public static void Trace(string text, ConsoleColor color = default)
        {
            if (color == default) Console.ResetColor();
            else Console.ForegroundColor = color;

            if (__customPrefix.ContainsKey(color))
            {
                Console.Write("    " + __customPrefix[color] + " ");
                Console.ResetColor();
            }

            Console.WriteLine(text);
        }
    }

}
