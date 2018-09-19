using System;

namespace DotVVM.Compiler.Output
{
    internal class AggregatedOutputLogger : IOutputLogger
    {
        public IOutputLogger[] Logger { get; }

        public AggregatedOutputLogger(params IOutputLogger[] logger)
        {
            Logger = logger;
        }

        public void WriteVerbose(string message)
        {
            foreach (var outputLogger in Logger)
            {
                outputLogger.WriteVerbose(message);
            }
        }

        public void WriteInfo(string message)
        {
            foreach (var outputLogger in Logger)
            {
                outputLogger.WriteInfo(message);
            }
        }

        public void WriteWarning(string message)
        {
            foreach (var outputLogger in Logger)
            {
                outputLogger.WriteWarning(message);
            }
        }

        public void WriteResult(string result)
        {
            foreach (var outputLogger in Logger)
            {
                outputLogger.WriteResult(result);
            }
        }

        public void WriteError(string message)
        {
            foreach (var outputLogger in Logger)
            {
                outputLogger.WriteError(message);
            }
        }

        public void WriteError(Exception e)
        {
            WriteError(e.ToString());
        }
    }
}
