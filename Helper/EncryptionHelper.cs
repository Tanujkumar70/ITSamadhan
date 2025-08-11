/***************************************************************************************
 * File Name    : EncryptionHelper.cs
 * Description  : Provides utility methods for hashing, encrypting and decrypting data using
 *                symmetric and/or asymmetric algorithms (e.g., AES, RSA).
 * 
 * Developer    : Aman Kala
 * Created Date : August 6, 2025
 * Last Modified: August 6, 2025
 * Version      : 1.0.0
 * 
 * Revision History:
 * -------------------------------------------------------------------------------------
 * Date        | Developer     | Description
 * -------------------------------------------------------------------------------------
 * 2025-08-06  | Aman Kala   | Initial creation of the EncryptionHelper class.
 * 
 ****************************************************************************************/

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EMP.Helper
{
    public static class EncryptionHelper
    {
        private const string DefaultKey = "1234567890123456"; // 16 bytes = 128-bit key
        private const string DefaultIV = "6543210987654321";  // 16 bytes = 128-bit IV

        /// <summary>
        /// Encrypts the given plain text using AES algorithm.
        /// </summary>
        public static string Encrypt(string plainText, string key = DefaultKey, string iv = DefaultIV)
        {
            if (string.IsNullOrEmpty(plainText)) throw new ArgumentNullException(nameof(plainText));
            ValidateKeyAndIV(key, iv);

            using Aes aesAlg = Aes.Create();
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
            aesAlg.IV = Encoding.UTF8.GetBytes(iv);

            using MemoryStream msEncrypt = new MemoryStream();
            using CryptoStream csEncrypt = new CryptoStream(msEncrypt, aesAlg.CreateEncryptor(), CryptoStreamMode.Write);
            using StreamWriter swEncrypt = new StreamWriter(csEncrypt);
            swEncrypt.Write(plainText);

            return Convert.ToBase64String(msEncrypt.ToArray());
        }

        /// <summary>
        /// Decrypts the given cipher text using AES algorithm.
        /// </summary>
        public static string Decrypt(string cipherText, string key = DefaultKey, string iv = DefaultIV)
        {
            if (string.IsNullOrEmpty(cipherText)) throw new ArgumentNullException(nameof(cipherText));
            ValidateKeyAndIV(key, iv);

            using Aes aesAlg = Aes.Create();
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
            aesAlg.IV = Encoding.UTF8.GetBytes(iv);

            using MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText));
            using CryptoStream csDecrypt = new CryptoStream(msDecrypt, aesAlg.CreateDecryptor(), CryptoStreamMode.Read);
            using StreamReader srDecrypt = new StreamReader(csDecrypt);

            return srDecrypt.ReadToEnd();
        }

        /// <summary>
        /// Generates a new random AES key of the specified key size (e.g., 128, 192, 256 bits).
        /// </summary>
        public static string GenerateAesKey(int keySize = 128)
        {
            using Aes aesAlg = Aes.Create();
            aesAlg.KeySize = keySize;
            aesAlg.GenerateKey();
            return Convert.ToBase64String(aesAlg.Key);
        }

        /// <summary>
        /// Generates a new random AES Initialization Vector (IV).
        /// </summary>
        public static string GenerateAesIV()
        {
            using Aes aesAlg = Aes.Create();
            aesAlg.GenerateIV();
            return Convert.ToBase64String(aesAlg.IV);
        }

        /// <summary>
        /// Generates a random salt (used in password hashing).
        /// </summary>
        public static string GenerateSalt(int length = 16)
        {
            byte[] saltBytes = new byte[length];
            RandomNumberGenerator.Fill(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        /// <summary>
        /// Generates a SHA256 hash using password + salt.
        /// </summary>
        public static string GenerateHash(string password, string salt)
        {
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrEmpty(salt)) throw new ArgumentNullException(nameof(salt));

            using SHA256 sha256 = SHA256.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(password + salt);
            byte[] hashBytes = sha256.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashBytes)
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }

        /// <summary>
        /// Validates that the encryption key and IV are exactly 16 bytes (128 bits).
        /// </summary>
        private static void ValidateKeyAndIV(string key, string iv)
        {
            if (Encoding.UTF8.GetByteCount(key) != 16)
                throw new ArgumentException("Encryption key must be exactly 16 bytes (128-bit AES).");
            if (Encoding.UTF8.GetByteCount(iv) != 16)
                throw new ArgumentException("Initialization Vector (IV) must be exactly 16 bytes (128-bit AES).");
        }
    }
}
