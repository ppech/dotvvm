using System;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

namespace DotVVM.Compiler.Output
{
    public class FileOutputLogger : IOutputLogger
    {
        private string FilePath { get; }
        private Stopwatch Stopwatch { get; }

        public FileOutputLogger(string filePath, Stopwatch stopwatch)
        {
            if(string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("File path cannot be empty.", nameof(filePath));
            FilePath = filePath;
            Stopwatch = stopwatch;

            File.Create(FilePath).Dispose();
        }

        public void WriteVerbose(string message)
        {
            WriteInfo(message);
        }

        public void WriteInfo(string message)
        {
            WriteToFile($"#{GetFormed(message)}");
        }

        public void WriteWarning(string message)
        {
            WriteInfo(message);
        }

        public void WriteResult(string result)
        {
            WriteToFile(result);
        }

        public void WriteError(string message)
        {
            WriteToFile($"!{message}");
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

        private void WriteToFile(string message)
        {
            using (var streamWriter = new StreamWriter(FilePath))
            {
                streamWriter.WriteLine(message);
            }
        }
    }
}
