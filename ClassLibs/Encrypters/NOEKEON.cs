using System;
using System.Linq;
using System.Text;
using MyClassLib;

namespace EncryptingClasses
{
    public class NOEKEON : BasicEncrypter
    {
        private uint[] keys = new uint[4];
        
        private static readonly int Nr = 16;

        private static readonly uint[] nullVector =
        {
            0x00, 0x00, 0x00, 0x00
		};

        private static readonly uint[] roundConstants =
        {
            0x80, 0x1b, 0x36, 0x6c,
            0xd8, 0xab, 0x4d, 0x9a,
            0x2f, 0x5e, 0xbc, 0x63,
            0xc6, 0x97, 0x35, 0x6a,
            0xd4
        };
        public override string Decrypt(string data)
        {
            Thetha(keys, nullVector);
            
            var result = new StringBuilder();
            var uints = GetUintsfromBytestr(data);
            for (int i = 0; i < uints.Length;)
            {
                uint[] block = { uints[i++], uints[i++], uints[i++], uints[i++] };
                for (int j = Nr; j > 0; --j)
                    Round(block, keys, 0, roundConstants[j]);
                Thetha(block, keys);
                block[0] ^= roundConstants[0];
                result.Append(UintstoStr(block));
            }
            return result.ToString();
        }

        public override string Encrypt(string data)
        {
            var result = new StringBuilder();
            var uints = GetUInts(data);
            for (int i = 0; i < uints.Length;)
            {
                uint[] block = { uints[i++], uints[i++], uints[i++], uints[i++] };
                for (int j = 0; j < Nr; ++j)
                    Round(block, keys, roundConstants[j], 0);
                block[0] ^= roundConstants[Nr];
                Thetha(block, keys);
                result.Append(UintstoBytestr(block));
            }
            return result.ToString();
        }

        public override void SetKey(string key, int _)
        {
            byte[] bytes = KeyValidator.Validate128Bit(key);
            for (int i = 0; i < 4; ++i)
                keys[i] = BitConverter.ToUInt32(bytes, i * 4);
        }

        private uint[] GetUintsfromBytestr(string data)
        {
            var split = data.Split();
            var bytes = new byte[split.Length - 1];
            for (int i = 0; i < bytes.Length; ++i)
            {
                var byt = BitConverter.GetBytes(int.Parse(split[i]));
                bytes[i] = byt[0];
            }
            uint[] result = new uint[bytes.Length / 4];
            for (int i = 0; i < result.Length; ++i)
                result[i] = BitConverter.ToUInt32(bytes, i * 4);
            return result;
        }
        private uint[] GetUInts(string data)
        {
            var bytes = Encoding.Unicode.GetBytes(data);
            var zerobyte = BitConverter.GetBytes(false)[0];
            while (bytes.Length % 16 != 0)
                bytes = bytes.Append(zerobyte).ToArray();
            uint[] result = new uint[bytes.Length / 4];
            for (int i = 0; i < result.Length; ++i)
                result[i] = BitConverter.ToUInt32(bytes, i * 4);
            return result;
        }

        private string UintstoBytestr(uint[] data)
        {
            var builder = new StringBuilder();
            for (int i = 0; i < data.Length; ++i)
            {
                var bytes = BitConverter.GetBytes(data[i]);
                for (int j = 0; j < bytes.Length; ++j)
                {
                    builder.Append(bytes[j]);
                    builder.Append(' ');
                }
            }
            return builder.ToString();
        }

        private string UintstoStr(uint[] data)
        {
            var builder = new StringBuilder();
            for (int i = 0; i < data.Length; ++i)
            {
                var bytes = BitConverter.GetBytes(data[i]);
                builder.Append(BitConverter.ToChar(bytes, 0));
                builder.Append(BitConverter.ToChar(bytes, 2));
            }
            return builder.ToString();
        }

        private void Round(uint[] block, uint[] key, uint const1, uint const2) 
        {
            block[0] ^= const1;
            Thetha(block, key);
            block[0] ^= const2;
            Pi1(block);
            Gamma(block);
            Pi2(block);
        }
        private void Thetha(uint[] block, uint[] key)
        {
            uint temp = block[0] ^ block[2];
            temp ^= temp >> 8 ^ temp << 8;
            block[1] ^= temp;
            block[3] ^= temp;
            for (int i = 0; i < 4; ++i)
                block[i] ^= key[i];
            temp = block[1] ^ block[3];
            temp ^= temp >> 8 ^ temp << 8;
            block[0] ^= temp;
            block[2] ^= temp;
        }
        private void Gamma(uint[] block)
        {
            block[1] ^= ~block[3] & ~block[2];
            block[0] ^= block[2] & block[1];

            uint tmp = block[3];
            block[3] = block[0];
            block[0] = tmp;
            block[2] ^= block[0] ^ block[1] ^ block[3];

            block[1] ^= ~block[3] & ~block[2];
            block[0] ^= block[2] & block[1];
        }
        private void Pi1(uint[] block)
        {
            block[1] = rotl(block[1], 1);
            block[2] = rotl(block[2], 5);
            block[3] = rotl(block[3], 2);
        }

        private void Pi2(uint[] block)
        {
            block[1] = rotl(block[1], 31);
            block[2] = rotl(block[2], 27);
            block[3] = rotl(block[3], 30);
        }

        private static uint rotl(uint x, int y)
        {
            return (x << y) | (x >> (32 - y));
        }
    }
}
