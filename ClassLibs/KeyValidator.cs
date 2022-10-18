using System;
using System.Collections.Generic;
using System.Text;

namespace EncryptingClasses
{
    class KeyValidator
    {
        static WrongKeyValue error = new WrongKeyValue("Key must be a string of characters from selected alphabet");
        static WrongKeyValue NOEKEONError = new WrongKeyValue("Key must contain 8 UTF-16 characters");
        static public string ValidateHaslo(string key, List<char> alphabet)
        {
            if (key.Length > 0)
            {
                foreach (var character in key)
                {
                    if (alphabet.IndexOf(character) == -1)
                        throw error;
                }
                return key;
            }
            throw error;
        }
        static public byte[] Validate128Bit(string key)
        {
            if (key.Length != 8)
                throw NOEKEONError;
            return Encoding.Unicode.GetBytes(key);
        }
    }
}
