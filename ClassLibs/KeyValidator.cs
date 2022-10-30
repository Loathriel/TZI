using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyClassLib
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
        static public KeyValuePair<long, long> ValidatePQ(string key)
        {
            var values = key.Split(new char[] { ' ', ';', ',', '.'});
            var numbers = values.Where(x => x.Length > 0).ToArray();
            if (numbers.Length != 2)
                throw new WrongKeyValue("Key must contain two separated positive integers.");
            long p, q;
            if (!(long.TryParse(numbers[0], out p) & long.TryParse(numbers[1], out q)))
                throw new WrongKeyValue("Key values must be integers.");
            if (!(p % 4 == 3 & q % 4 == 3))
                throw new WrongKeyValue("Key values must have property val % 4 = 3");
            if (!(p.IsPrime() & q.IsPrime()))
                throw new WrongKeyValue("Key values must be prime");
            if (p == q)
                throw new WrongKeyValue("Key values must be different numbers");
            return new KeyValuePair<long, long>(p, q);
        }
    }
}
