using System;

namespace DotVVM.Compiler.Output
{
    public interface IOutputLogger
    {
        void WriteVerbose(string message);
        void WriteInfo(string message);
        void WriteWarning(string message);
        void WriteResult(string result);
        void WriteError(string message);
        void WriteError(Exception e);
    }
}
