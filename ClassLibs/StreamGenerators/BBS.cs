using MyClassLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var keyPair = KeyValidator.ValidatePQ(key);
            long p = keyPair.Key, q = keyPair.Value;
            long r;
            r = n = p * q;
            var randomizer = new Random();
            while (!r.IsCoprime(n))
                r = randomizer.Next(2, (int)n);
            current = (r * r).Mod(n);
        }
    }
}
