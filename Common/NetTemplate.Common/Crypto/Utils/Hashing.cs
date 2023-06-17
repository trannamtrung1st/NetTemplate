using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace NetTemplate.Common.Crypto.Utils
{
    public static class Hashing
    {
        public static MD5 CreateMd5() => MD5.Create();

        public static byte[] Hash(this HashAlgorithm hashAlgorithm, Stream stream)
        {
            return hashAlgorithm.ComputeHash(stream);
        }

        public static byte[] HashPasswordPbkdf2(string password, byte[] salt,
            KeyDerivationPrf keyDerivationPrf = KeyDerivationPrf.HMACSHA1,
            int iterCount = 100000,
            int numBytesRequested = 256 / 8)
        {
            return KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: keyDerivationPrf,
                iterationCount: iterCount,
                numBytesRequested: numBytesRequested);
        }
    }
}
