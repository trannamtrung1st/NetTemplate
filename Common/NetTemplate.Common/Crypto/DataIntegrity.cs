using System.Security.Cryptography;

namespace NetTemplate.Common.Crypto
{
    public static class DataIntegrity
    {
        public static HMACMD5 CreateHmacMd5(byte[] key = null) => key != null ? new HMACMD5(key) : new HMACMD5();
        public static HMACSHA1 CreateHmacSha1(byte[] key = null) => key != null ? new HMACSHA1(key) : new HMACSHA1();
        public static HMACSHA256 CreateHmacSha256(byte[] key = null) => key != null ? new HMACSHA256(key) : new HMACSHA256();
        public static HMACSHA384 CreateHmacSha384(byte[] key = null) => key != null ? new HMACSHA384(key) : new HMACSHA384();
        public static HMACSHA512 CreateHmacSha512(byte[] key = null) => key != null ? new HMACSHA512(key) : new HMACSHA512();

        public static void Sign(this HMAC hmac, Stream inStream, Stream outStream)
        {
            byte[] hashValue = hmac.ComputeHash(inStream);
            // Reset inStream to the beginning of the file.
            inStream.Position = 0;
            // Write the computed hash value to the output file.
            outStream.Write(hashValue, 0, hashValue.Length);
            // Copy the contents of the sourceFile to the destFile.
            int bytesRead;
            // read 1K at a time
            byte[] buffer = new byte[1024];
            do
            {
                // Read from the wrapping CryptoStream.
                bytesRead = inStream.Read(buffer, 0, 1024);
                outStream.Write(buffer, 0, bytesRead);
            } while (bytesRead > 0);
        }

        public static bool Verify(this HMAC hmac, Stream inStream)
        {
            bool invalid = false;
            // Initialize the keyed hash object.
            // Create an array to hold the keyed hash value read from the file.
            byte[] storedHash = new byte[hmac.HashSize / 8];
            // Create a FileStream for the source file.
            // Read in the storedHash.
            inStream.Read(storedHash, 0, storedHash.Length);
            // Compute the hash of the remaining contents of the file.
            // The stream is properly positioned at the beginning of the content,
            // immediately after the stored hash value.
            byte[] computedHash = hmac.ComputeHash(inStream);
            // compare the computed hash with the stored value

            for (int i = 0; i < storedHash.Length; i++)
            {
                if (computedHash[i] != storedHash[i])
                {
                    invalid = true;
                }
            }

            return !invalid;
        }
    }
}
