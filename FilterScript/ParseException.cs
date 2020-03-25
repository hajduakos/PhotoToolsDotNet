using System;

namespace FilterScript
{
    public class ParseException : Exception
    {
        public int Line { get; private set; }

        public ParseException(int line, string message = null) : base(message)
        {
            this.Line = line;
        }

        public override string ToString()
        {
            return "Line " + Line + ": " + base.Message;
        }
    }
}
