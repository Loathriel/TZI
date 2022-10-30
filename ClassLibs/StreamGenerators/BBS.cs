using MyClassLib;
using System;

namespace ClassLibs.StreamGenerators
{
    public class BBS : BasicSGenerator
    {
        long n, current;

        public BBS() { }
        public override char NextChar()
        {
            int result = 0;
            for (int i = 0; i < 16; ++i) 
            {
                int bit = (int)(current % 2);
                result ^= (bit << i);
                current = (current * current).Mod(n);
            }
            return (char)result;
        }

        public override void setValues(string key)
        {
            var keys = KeyValidator.ValidatePQR(key);
            long r = keys[2];
            n = keys[0] * keys[1];
            current = (r * r).Mod(n);
        }

        public static string GenerateKey(string values)
        {
            var keyPair = KeyValidator.ValidatePQ(values);
            long p = keyPair.Key, q = keyPair.Value;
            long r, n;
            r = n = p * q;
            var randomizer = new Random();
            while (!r.IsCoprime(n))
                r = randomizer.Next(2, (int)n);
            return $"{p},{q},{r}";
        }
    }
}
