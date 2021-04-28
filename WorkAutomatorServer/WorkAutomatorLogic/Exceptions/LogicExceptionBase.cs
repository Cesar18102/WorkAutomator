using System;

namespace WorkAutomatorLogic.Exceptions
{
    public class LogicExceptionBase : Exception 
    {
        public LogicExceptionBase() : base() { }
        public LogicExceptionBase(string message) : base(message) { }
    }
}
