using ClassLibs.StreamGenerators;
using MyClassLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptingClasses
{
    public class StreamBasedEncrypter : BasicEncrypter
    {
        BasicSGenerator generator;

        public StreamBasedEncrypter(BasicSGenerator generator)
        {
            this.generator = generator;
        }
        private string Transform(string data)
        {
            StringBuilder builder = new StringBuilder();
            foreach (char c in data)
            {
                char toAdd = (char)(c ^ generator.NextChar());
                builder.Append(toAdd);
            }
            return builder.ToString();
        }
        public override string Decrypt(string data)
        {
            data = GetCharsfromBytestr(data);
            return Transform(data);
        }

        public override string Encrypt(string data)
        {
            return StringtoBytestr(Transform(data));
        }

        public override void SetKey(string key, int _)
        {
            generator.setValues(key);
        }
        private string GetCharsfromBytestr(string data)
        {
            var result = new StringBuilder();
            var split = data.Split();
            for (int i = 0; i < split.Length - 1; ++i)
                result.Append((char)ushort.Parse(split[i]));
            return result.ToString();
        }

        private string StringtoBytestr(string data)
        {
            var builder = new StringBuilder();
            for (int i = 0; i < data.Length; ++i)
            {
                builder.Append(Convert.ToUInt16(data[i]));
                builder.Append(' ');
            }
            return builder.ToString();
        }

    }
}
