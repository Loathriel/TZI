using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EncryptingClasses
{
    public class NOEKEON : BasicEncrypter
    {
        private char[] keys, decryptkeys;

        private static readonly int Nr = 16;

        private static readonly char NULL = (char)0;

        private static readonly char[] nullVector =
        {
            NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL
        };

        private static readonly UInt16[] roundConstants1 =
        {
            0x8, 0x1, 0x3, 0x6,
            0xd, 0xa, 0x4, 0x9,
            0x2, 0x5, 0xb, 0x6,
            0xc, 0x9, 0x3, 0x6,
            0xd
        };
        private static readonly UInt16[] roundConstants2 =
        {
            0x0, 0xb, 0x6, 0xc,
            0x8, 0xb, 0xd, 0xa,
            0xf, 0xe, 0xc, 0x3,
            0x6, 0x7, 0x5, 0xa,
            0x4
        };
        public override string Decrypt(string data)
        {
            throw new NotImplementedException();
        }

        public override string Encrypt(string data)
        {
            var result = new StringBuilder();
            var chars = data.ToCharArray();
            while (chars.Length % 8 != 0)
                chars = chars.Append(NULL).ToArray();
            for (int i = 0; i < chars.Length; i += 8)
            {
                int index = i;
                char[] block =
                {
                    chars[index++], chars[index++], 
                    chars[index++], chars[index++],
                    chars[index++], chars[index++], 
                    chars[index++], chars[index++]
                };
                for (int j = 0; j < Nr; ++j)
                   Round(block, keys, (char)roundConstants1[j], (char)roundConstants2[j], NULL, NULL);
                block[0] ^= (char)roundConstants1[Nr];
                block[1] ^= (char)roundConstants2[Nr];
                Thetha(block, keys);
                for (index = i; index < i + 8; ++index)
                    result.Append(block[index]);
            }
            return result.ToString();
        }

        public override void SetKey(string key, int k = 0)
        {
            decryptkeys = keys = KeyValidator.Validate128Bit(key);
            Thetha(decryptkeys, nullVector);
        }

        private void Round(char[] block, char[] key, char const11, char const12, char const21, char const22)
        {
            block[0] ^= const11;
            block[1] ^= const12;
            Thetha(block, key);
            block[0] ^= const21;
            block[1] ^= const22;
            Pi1(block);
            Gamma(block);
            Pi2(block);
        }
        private void Thetha(char[] block, char[] key)
        {
            char temp = (char)(block[0] ^ block[2]);
            temp ^= (char)(temp >> 8 ^ temp << 8);
            block[1] ^= temp;
            block[3] ^= temp;
            for (int i = 0; i < 4; ++i)
                block[i] ^= key[i];
            temp = (char)(block[1] ^ block[3]);
            temp ^= (char)(temp >> 8 ^ temp << 8);
            block[0] ^= temp;
            block[2] ^= temp;
        }
        private void Gamma(char[] block)
        {
            block[2] ^= (char)(~block[6] & ~block[4]);
            block[3] ^= (char)(~block[7] & ~block[5]);

            block[0] ^= (char)(block[4] & block[2]);
            block[1] ^= (char)(block[5] & block[3]);

            (block[0], block[6], block[1], block[7]) = 
                (block[6], block[0], block[7], block[1]);

            block[4] ^= (char)(block[0] ^ block[2] ^ block[6]);
            block[5] ^= (char)(block[1] ^ block[3] ^ block[7]);

            block[2] ^= (char)(~block[6] & ~block[4]);
            block[3] ^= (char)(~block[7] & ~block[5]);

            block[0] ^= (char)(block[4] & block[2]);
            block[1] ^= (char)(block[5] & block[3]);
        }
        private void Pi1(char[] block)
        {
            uint[] uints =
            {
                CharsToUint(block[2], block[3]),
                CharsToUint(block[4], block[5]),
                CharsToUint(block[6], block[7])
            };
            uints[0] = rotl(uints[0], 1);
            uints[1] = rotl(uints[1], 5);
            uints[2] = rotl(uints[2], 2);
            char[][] res =
            {
                UintToChars(uints[0]),
                UintToChars(uints[1]),
                UintToChars(uints[2])
            };
            (block[2], block[3]) = (res[0][0], res[0][1]);
            (block[4], block[5]) = (res[1][0], res[1][1]);
            (block[6], block[7]) = (res[2][0], res[2][1]);
        }
        private void Pi2(char[] block)
        {
            uint[] uints =
            {
                CharsToUint(block[2], block[3]),
                CharsToUint(block[4], block[5]),
                CharsToUint(block[6], block[7])
            };
            uints[0] = rotl(uints[0], 31);
            uints[1] = rotl(uints[0], 27);
            uints[2] = rotl(uints[0], 30);
            char[][] res =
            {
                UintToChars(uints[0]),
                UintToChars(uints[1]),
                UintToChars(uints[2])
            };
            (block[2], block[3]) = (res[0][0], res[0][1]);
            (block[4], block[5]) = (res[1][0], res[1][1]);
            (block[6], block[7]) = (res[2][0], res[2][1]);
        }

        private uint CharsToUint(char c1, char c2)
        {
            var bytes = Encoding.Unicode.GetBytes($"{c1}{c2}");
            swap(bytes);
            return BitConverter.ToUInt32(bytes, 0);
        }

        private char[] UintToChars(uint u)
        {
            var bytes = BitConverter.GetBytes(u);
            swap(bytes);
            char[] result = 
            {
                BitConverter.ToChar(bytes, 0),
                BitConverter.ToChar(bytes, 2)
            };
            return result;
        }

        private void swap(byte[] bytes)
        {
            (bytes[1], bytes[0]) = (bytes[0], bytes[1]);
            (bytes[3], bytes[2]) = (bytes[2], bytes[3]);
        }
        private static uint rotl(uint x, int y)
        {
            return (x << y) ^ (x >> (32 - y));
        }
    }
}
