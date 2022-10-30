using System;

namespace MyClassLib
{
    public class WrongKeyValue: Exception
    {
        public WrongKeyValue(string message): base(message)
        { }
    }
}
