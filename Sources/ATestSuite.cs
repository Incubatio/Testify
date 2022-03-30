using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace testify
{
    public abstract class ATestSuite
    {
        public const string EXPECTED_EXCEPTION = "Expected Exception";

        private Action<object, object> _assert;
        private Action<string, ConsoleColor> _trace;

        public TestResult Result;
        private List<Task> _tasks;
        public List<List<(string, ConsoleColor)>> Outputs => Result.Outputs;

        public int ExitCode => Result.FailingCount > 0 ? 1 : 0;

        public ATestSuite() : this(Tools.Assert, Tools.Trace) { }

        public ATestSuite(Action<object, object> assert, Action<string, ConsoleColor> trace)
        {
            _tasks = new List<Task>();
            Result = new TestResult();
            _assert = assert;
            _trace = trace;
        }

        public void Describe(string featureDescription, Action testSuiteFn)
        {
            //_trace("Running: " + featureDescription, default);
            var index = Outputs.Count;
            Outputs.Add(new List<(string, ConsoleColor)>());
            Outputs[index].Add(("", default));
            Outputs[index].Add(("  " + featureDescription, default));
            Outputs[index].Add(("  " + new string('-', featureDescription.Length), default));

            testSuiteFn();
        }

        public void It(string testDescription)
        {
            var output = Outputs[Outputs.Count - 1];
            Result.NotImplementedCount++;
            output.Add((testDescription, ConsoleColor.DarkYellow));
        }

        public void It(string testDescription, Action<Action<object, object>> testFn)
        {
            var output = Outputs[Outputs.Count - 1];
            var startTime = DateTime.Now;
            try
            {
                testFn(Tools.Assert);
                var duration = Math.Ceiling((DateTime.Now - startTime).TotalMilliseconds * 100) * 0.01;
                Result.PassingCount++;
                output.Add(( "("+ duration + "ms)" + " It " + testDescription, ConsoleColor.DarkGreen));
            }
            catch (Exception e)
            {
                Result.FailingCount++;
                output.Add(("It " + testDescription, ConsoleColor.DarkRed));
                output.Add(("      Error: " + e.Message, ConsoleColor.Red));
                output.Add((e.StackTrace.Replace("   ", "      "), default));
            }
        }

        public void XIt(string testDescription, Action<Action<object, object>> testFn)
        {
            var output = Outputs[Outputs.Count - 1];
            Result.ExcludedCount++;
            output.Add((testDescription, ConsoleColor.DarkBlue));
        }

        public virtual void Run()
        {
            throw new NotImplementedException();
        }
    }
}
