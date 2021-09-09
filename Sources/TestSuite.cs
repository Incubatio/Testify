using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace testify
{
    class TestSuite
    {
        public TaskCompletionSource<bool> CompletionToken;

        private List<List<(String, ConsoleColor)>> _outputs;

        private int _runningCount = 0;

        private int _passingCount = 0;
        private int _failingCount = 0;
        private int _excludedCount = 0;
        private int _notImplementedCount = 0;
        private Action<object, object> _assert;
        private Action<string, ConsoleColor> _trace;

        private DateTime _startTime;


        public TestSuite() : this(Tools.Assert, Tools.Trace) { }

        public TestSuite(Action<object, object> assert, Action<string, ConsoleColor> trace)
        {
            _startTime = DateTime.Now;
            CompletionToken = new TaskCompletionSource<bool>();
            _outputs = new List<List<(String, ConsoleColor)>>();
            _assert = assert;
            _trace = trace;
        }


        public async void Describe(string featureDescription, Action testSuiteFn)
        {
            //_trace("Running: " + featureDescription, default);
            _runningCount += 1;
            var index = _outputs.Count;
            _outputs.Add(new List<(string, ConsoleColor)>());
            _outputs[index].Add(("", default));
            _outputs[index].Add(("  " + featureDescription, default));
            _outputs[index].Add(("  " + new string('-', featureDescription.Length), default));
            testSuiteFn();

            await Task.Delay(5);
            _runningCount -= 1;
            if (_runningCount == 0)
            {
                for (var i = 0; i < _outputs.Count; i++)
                    foreach ((string text, ConsoleColor color) in _outputs[i])
                        _trace(text, color);

                _trace("", default);
                _trace("> " + _passingCount + " Passing", ConsoleColor.Green);
                if (_failingCount > 0)
                    _trace("> " + _failingCount + " Failing", ConsoleColor.Red);
                if (_excludedCount > 0)
                    _trace("> " + _excludedCount + " Excluded", ConsoleColor.Blue);
                if (_notImplementedCount > 0)
                    _trace("> " + _notImplementedCount + " NotImplemented", ConsoleColor.Yellow);
                var endTime = DateTime.Now;
                _trace("> Time elapsed: " + (endTime - _startTime).Milliseconds + "ms", default);
                CompletionToken.SetResult(true);
            }
        }

        public void It(string testDescription)
        {
            var output = _outputs[_outputs.Count - 1];
            _notImplementedCount++;
            output.Add((testDescription, ConsoleColor.DarkYellow));
        }

        public void It(string testDescription, Action<Action<object, object>> testFn)
        {
            var output = _outputs[_outputs.Count - 1];
            try
            {
                testFn(Tools.Assert);
                _passingCount++;
                output.Add((testDescription, ConsoleColor.DarkGreen));
            }
            catch (Exception e)
            {
                _failingCount++;
                output.Add(("It " + testDescription, ConsoleColor.DarkRed));
                output.Add(("      Error: " + e.Message, ConsoleColor.Red));
                output.Add((e.StackTrace.Replace("   ", "      "), default));
            }
        }

        public void XIt(string testDescription, Action<Action<object, object>> testFn)
        {
            var output = _outputs[_outputs.Count - 1];
            _excludedCount++;
            output.Add((testDescription, ConsoleColor.DarkBlue));
        }

        public void Wait() => CompletionToken.Task.Wait();

    }
}
