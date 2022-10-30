using System;
using System.Collections.Generic;
using System.Text;
using MyClassLib;

namespace EncryptingClasses
{
    public class VigenereEncrypter : BasicEncrypter
    {
        private string haslo;
        protected List<char> alphabet;
        public int alphabetLength;

        public VigenereEncrypter()
        {
            alphabet = new List<char>()
            {
                'а', 'б', 'в', 'г', 'ґ', 'д', 'е', 'є', 'ж', 'з', 'и',
                'і', 'ї', 'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с',
                'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ь', 'ю', 'я'
            };
            alphabetLength = alphabet.Count;
        }

        private string Transform(string data, Func<int, int, int> func)
        {
            data = data.ToLower();
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
