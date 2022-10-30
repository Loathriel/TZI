using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLib
{
    public static class MyMath
    {
        public static bool IsPrime(this long number)
        {
            if (number <= 1) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            var boundary = (int)Math.Floor(Math.Sqrt(number));

            for (int i = 3; i <= boundary; i += 2)
                if (number % i == 0)
                    return false;

            return true;
        }

        public static long gcd(this long a, long b)
        {
            if (a == 0 || b == 0)
                return 0;

            if (a == b)
                return a;

            if (a > b)
                return b.gcd(a - b);

            return a.gcd(b - a);
        }

        public static bool IsCoprime(this long n1, long n2)
        {
            return n1.gcd(n2) == 1;
        }

        public static long Mod(this long k, long n)
        {
            return ((k %= n) < 0) ? k + n : k;
        }
    }
}
