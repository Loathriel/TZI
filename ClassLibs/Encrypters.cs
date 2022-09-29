using System.Text;
using System.Collections.Generic;
using System;
using System.Linq;

namespace EncryptingClasses
{
    public abstract class BasicEncrypter
    {
        protected List<char> alphabet;
        public int alphabetLength;

        protected BasicEncrypter()
        {
            alphabet = new List<char>() 
            {
                'а', 'б', 'в', 'г', 'ґ', 'д', 'е', 'є', 'ж', 'з', 'и',
                'і', 'ї', 'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с',
                'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ь', 'ю', 'я'
            };
            alphabetLength = alphabet.Count;
        }

        protected int Mod(int k, int n)
        {
            return ((k %= n) < 0) ? k + n : k;
        }
        public abstract string Encrypt(string data);
        public abstract string Decrypt(string data);
        public abstract void SetKey(string _haslo, int k);
    }

    public class CeasarEncrypter : BasicEncrypter
    {
        private Dictionary<char, char> cipherTable;
        private Dictionary<char, char> decipherTable;

        public CeasarEncrypter() : base() { }

        public override void SetKey(string haslo, int k)
        {
            KeyValidator.ValidateHaslo(haslo, alphabet);
            if (haslo.Distinct().Count() != haslo.Length)
                throw new WrongKeyValue("keyword must not contain duplicate characters");
            cipherTable = new Dictionary<char, char>();
            decipherTable = new Dictionary<char, char>();
            int hasloLen = haslo.Length;
            var copy = new List<char>(alphabet);
            
            for (int i = 0; i < hasloLen; ++i)
            {
                cipherTable.Add(alphabet[k], haslo[i]);
                decipherTable.Add(haslo[i], alphabet[k]);
                k = Mod(k + 1, alphabetLength);
                copy.Remove(haslo[i]);
            }

            for (int i = 0; i < copy.Count; ++i)
            {
                cipherTable.Add(alphabet[k], copy[i]);
                decipherTable.Add(copy[i], alphabet[k]);
                k = Mod(k + 1, alphabetLength);
            }
        }
        private string Transform(string data, Dictionary<char, char> table)
        {
            var builder = new StringBuilder();
            foreach (char c in data)
            {
                if (table.ContainsKey(c))
                    builder.Append(table[c]);
                else
                    builder.Append(c);
            }
            return builder.ToString();
        }
        public override string Encrypt(string data)
        {
            return Transform(data, cipherTable);
        }
        public override string Decrypt(string data)
        {

            return Transform(data, decipherTable);

        }
    }

    public class VigenereEncrypter : BasicEncrypter
    {
        private string haslo;

        public VigenereEncrypter() : base() { }

        private string Transform (string data, Func<int, int, int> func)
        {
            var builder = new StringBuilder();
            int haslo_index = 0;
            foreach (char c in data.ToLower())
            {
                var index = alphabet.IndexOf(c);
                if (index == -1) { builder.Append(c); }
                else
                {
                    var added = alphabet.IndexOf(haslo[haslo_index]);
                    var newIndex = Mod(func(index, added), alphabetLength);
                    builder.Append(alphabet[newIndex]);
                    if (++haslo_index == haslo.Length)
                        haslo_index = 0;
                }
            }
            return builder.ToString();
        }
        public override string Decrypt(string data)
        {
            return Transform(data, (x, y) => x - y);
        }

        public override string Encrypt(string data)
        {
            return Transform(data, (x, y) => x + y);
        }

        public override void SetKey(string _haslo, int _)
        {
            haslo = KeyValidator.ValidateHaslo(_haslo, alphabet);
        }
    }
}
