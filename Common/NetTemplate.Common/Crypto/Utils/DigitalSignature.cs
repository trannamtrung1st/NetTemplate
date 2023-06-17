using System.Security.Cryptography;

namespace NetTemplate.Common.Crypto.Utils
{
    public static class DigitalSignature
    {
        // [IMPORTANT] Import/Export: ToXmlString(), ExportParamaters/ImportParameters
        public static RSACryptoServiceProvider CreateRsa() => new RSACryptoServiceProvider();

        public static byte[] EncryptData(this RSACryptoServiceProvider rsa,
            byte[] dataToEncrypt, RSAParameters rsaKey, bool doOAEPPadding = false)
        {
            byte[] encryptedData;

            //Import the RSA Key information. This only needs
            //toinclude the public key information.
            rsa.ImportParameters(rsaKey);

            //Encrypt the passed byte array and specify OAEP padding.  
            //OAEP padding is only available on Microsoft Windows XP or
            //later.  
            encryptedData = rsa.Encrypt(dataToEncrypt, doOAEPPadding);
            return encryptedData;
        }

        public static byte[] DecryptData(this RSACryptoServiceProvider rsa,
            byte[] dataToDecrypt, RSAParameters rsaKey, bool doOAEPPadding = false)
        {
            byte[] decryptedData;
            //Import the RSA Key information. This needs
            //to include the private key information.
            rsa.ImportParameters(rsaKey);

            //Decrypt the passed byte array and specify OAEP padding.  
            //OAEP padding is only available on Microsoft Windows XP or
            //later.  
            decryptedData = rsa.Decrypt(dataToDecrypt, doOAEPPadding);
            return decryptedData;
        }
    }
}
