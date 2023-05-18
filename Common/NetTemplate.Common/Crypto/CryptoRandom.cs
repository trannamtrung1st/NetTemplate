using System.Security.Cryptography;

namespace NetTemplate.Common.Crypto
{
    public static class CryptoRandom
    {
        // [IMPORTANT] Random integer: RandomNumberGenerator.GetInt32()
        public static RandomNumberGenerator GetRng() => RandomNumberGenerator.Create();

        public static void FillRandom(this RandomNumberGenerator rng, byte[] arr)
        {
            rng.GetBytes(arr);
        }

        public static string RandomBase64String(this RandomNumberGenerator rng, byte[] arr)
        {
            rng.GetBytes(arr);
            return Convert.ToBase64String(arr);
        }

        public static byte[] GetSalt(this RandomNumberGenerator rng, int bitCount = 128)
        {
            byte[] salt = new byte[bitCount / 8];
            rng.GetNonZeroBytes(salt);
            return salt;
        }
    }
}
