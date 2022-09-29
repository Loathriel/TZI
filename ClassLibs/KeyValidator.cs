using System.Collections.Generic;

namespace EncryptingClasses
{
    class KeyValidator
    {
        static WrongKeyValue error = new WrongKeyValue("Key must be a string of characters from selected alphabet");
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
    }
}
