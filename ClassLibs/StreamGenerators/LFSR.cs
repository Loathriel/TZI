using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibs.StreamGenerators
{
    public class LFSR : BasicSGenerator
    {
        BitArray bits;
        bool[] startValues = new bool[] 
        { 
            false, true, false, false, false, 
            false, false, false, false, false, true 
        };

        public LFSR() { }
        public override char NextChar()
        {
            int result = 0;
            for (int i = 0; i < 16; ++i)
            {
                bool newBit = bits[10] ^ bits[1] ^ true;
                int toAdd = bits[0] ? 1 : 0;
                result += toAdd << i;
                RShift(newBit);
            }
            return (char)result;
        }

        public override void setValues(string _) 
        {
            resetArray();
        }

        private void RShift(bool value)
        {
            for (int i = 0; i < 10; ++i)
                bits[i] = bits[i + 1];
            bits[10] = value;
        }

        private void resetArray()
        {
            bits = new BitArray(startValues);
        }
    }
}
