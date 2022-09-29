using System;

namespace EncryptingClasses
{
    public class WrongKeyValue: Exception
    {
        public WrongKeyValue(string message): base(message)
        { }
    }
}
