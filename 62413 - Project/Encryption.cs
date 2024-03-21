using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace _62413___Project
{
    internal class Encryption
    {
        /// <summary>
        /// Encrypts a given plaintext string using a specified password.
        /// </summary>
        /// <param name="plainText">The plaintext string to encrypt.</param>
        /// <param name="password">The password used for encryption.</param>
        /// <returns>The encrypted string in Base64 format.</returns>
        public static string EncryptString(string plainText, string password)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            // Use a salt value and specify the hash algorithm and number of iterations
            byte[] salt = Encoding.UTF8.GetBytes("this is my salt");
            int iterations = 10000; // Adjust the number of iterations/rounds to make it more secure
            HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA256; // Specify the hash algorithm

            using (Rfc2898DeriveBytes rfcKey = new Rfc2898DeriveBytes(password, salt, iterations, hashAlgorithm))
            {
                Aes encryptor = Aes.Create();
                encryptor.Key = rfcKey.GetBytes(32); // 32 bytes for AES-256
                encryptor.IV = rfcKey.GetBytes(16); // 16 bytes for AES IV

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(plainTextBytes, 0, plainTextBytes.Length);
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        /// <summary>
        /// Decrypts a given encrypted text using a specified password.
        /// </summary>
        /// <param name="encryptedText">The encrypted text in Base64 format to decrypt.</param>
        /// <param name="password">The password used for decryption.</param>
        /// <returns>The decrypted string.</returns>
        public static string DecryptString(string encryptedText, string password)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            byte[] salt = Encoding.UTF8.GetBytes("this is my salt");
            int iterations = 10000;
            HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA256;

            using (Rfc2898DeriveBytes rfcKey = new Rfc2898DeriveBytes(password, salt, iterations, hashAlgorithm))
            {
                Aes encryptor = Aes.Create();
                encryptor.Key = rfcKey.GetBytes(32);
                encryptor.IV = rfcKey.GetBytes(16);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(encryptedBytes, 0, encryptedBytes.Length);
                    }
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }
    }
}
