using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyClassLib;

namespace EncryptingClasses
{
    public class CeasarEncrypter : BasicEncrypter
    {
        private Dictionary<char, char> cipherTable;
        private Dictionary<char, char> decipherTable;
        protected List<char> alphabet;
        public int alphabetLength;

        public CeasarEncrypter() 
        {
            alphabet = new List<char>()
            {
                'а', 'б', 'в', 'г', 'ґ', 'д', 'е', 'є', 'ж', 'з', 'и',
                'і', 'ї', 'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с',
                'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ь', 'ю', 'я'
            };
            alphabetLength = alphabet.Count;
        }

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
            data = data.ToLower();
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
}
