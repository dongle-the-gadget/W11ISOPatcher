using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W11ISO
{
    public static class Logging
    {
        public enum LogLevel
        {
            Critical,
            Error,
            Warning,
            Information,
            Debug
        }

        private static void Log(LogLevel level, string message)
        {
            switch (level)
            {
                case LogLevel.Critical:
                    Console.Write($"[{DateTimeOffset.Now}] ");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("[Critical]   ");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case LogLevel.Error:
                    Console.Write($"[{DateTimeOffset.Now}] ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("[Error]      ");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case LogLevel.Warning:
                    Console.Write($"[{DateTimeOffset.Now}] ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("[Warning]    ");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case LogLevel.Information:
                    Console.Write($"[{DateTimeOffset.Now}] ");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("[Information]");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case LogLevel.Debug:
                    Console.Write($"[{DateTimeOffset.Now}] ");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("[Debug]      ");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
            }

            Console.WriteLine(message);
        }

        public static void LogCritical(string message) => Log(LogLevel.Critical, message);
    }
}
