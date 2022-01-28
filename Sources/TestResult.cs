using System;
using System.Collections.Generic;

namespace testify
{
    public class TestResult
    {
        public List<List<(string text, ConsoleColor color)>> Outputs = new List<List<(string text, ConsoleColor color)>>();
        public int PassingCount = 0;
        public int FailingCount = 0;
        public int ExcludedCount = 0;
        public int NotImplementedCount = 0;
    }
}