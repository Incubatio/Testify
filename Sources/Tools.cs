using System;
using System.Collections.Generic;
using System.Text.Json;

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
