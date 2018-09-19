using System;
using System.Diagnostics;
using Newtonsoft.Json;

namespace DotVVM.Compiler.Output
{
    public class ConsoleOutputLogger : IOutputLogger
    {
        private Stopwatch Stopwatch { get; }

        public ConsoleOutputLogger(Stopwatch stopwatch)
        {
            Stopwatch = stopwatch;
        }
        public void WriteVerbose(string message)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Gray;
            WriteInfo(message);
            Console.ForegroundColor = color;
        }

        public void WriteInfo(string message)
        {
            Console.WriteLine($"#{GetFormed(message)}");
        }

        public void WriteWarning(string message)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            WriteInfo(message);
            Console.ForegroundColor = color;
        }

        public void WriteResult(string result)
        {
            Console.WriteLine(result);
        }

        public void WriteError(string message)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"!{message}");
            Console.WriteLine();
            Console.ForegroundColor = color;
        }

        public void WriteError(Exception e)
        {
            WriteInfo("Error occured!");
            var exceptionJson = JsonConvert.SerializeObject(e);
            WriteError(exceptionJson);
        }

        private string GetFormed(string message)
        {
            return $"{Stopwatch?.Elapsed}: {message}";
        }
    }
}
