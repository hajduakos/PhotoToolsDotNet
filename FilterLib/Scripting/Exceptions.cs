using System;

namespace FilterLib.Scripting
{
    public class ScriptException : Exception
    {
        public int Line { get; private set; }

        public ScriptException(int line, string message) : base(message) => this.Line = line;

        public ScriptException(int line, Exception innerException) : base(null, innerException) => this.Line = line;

        public override string ToString()
        {
            string result = "Exception at line " + Line + ": ";
            if (InnerException != null) result += InnerException.ToString();
            else result += Message;
            return result;
        }
    }

    public class SyntaxException : ScriptException
    {
        public SyntaxException(int line, string message) : base(line, message) { }
    }

    public class FilterNotFoundException : ScriptException
    {
        public FilterNotFoundException(int line, string message) : base(line, message) { }
    }

    public class ParamNotFoundException : ScriptException
    {
        public ParamNotFoundException(int line, string message) : base(line, message) { }
    }

    public class ParseException : ScriptException
    {
        public ParseException(int line, Exception innerException) : base(line, innerException) { }
    }

    public class ParamRangeException : ScriptException
    {
        public ParamRangeException(int line, string message) : base(line, message) { }
    }
}
