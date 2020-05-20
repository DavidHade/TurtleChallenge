using System;
using System.Collections.Generic;

namespace Domain.IO
{
    public static class Logger
    {
        public static void Log(List<KeyValuePair<string, ConsoleColor>> inputs)
        {
            foreach (var log in inputs)
            {
                Console.ForegroundColor = log.Value;
                Console.WriteLine(log.Key);
                Console.ResetColor();
            }
        }

        public static void Log(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
