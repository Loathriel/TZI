using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibs.StreamGenerators
{
    public abstract class BasicSGenerator
    {
        public abstract char NextChar();
        public abstract void setValues(string key);
    }
}
