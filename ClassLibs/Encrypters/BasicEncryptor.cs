namespace EncryptingClasses
{
    public abstract class BasicEncrypter
    {
        protected long Mod(long k, long n)
        {
            return ((k %= n) < 0) ? k + n : k;
        }
        public abstract string Encrypt(string data);
        public abstract string Decrypt(string data);
        public abstract void SetKey(string _haslo, int k);
    }
}
