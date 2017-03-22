﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AesCrypt
{
    /// <summary>
    /// AES + URL friendly Base64 (encrypting + encoding) / (decoding + decryption) mechanism
    /// </summary>
    public class AesCrypt
    {
        static readonly char[] padding = { '=' };

        private byte[] _key;
        private byte[] _iv;

        /// <summary>
        /// Secret Key for AES.
        /// </summary>
        public byte[] Key
        {
            get
            {
                if (_key == null)
                {
                    _key = GenerateRandomBytes(256);
                }

                return _key;
            }
        }

        /// <summary>
        /// Initialization Vector for AES.
        /// </summary>
        public byte[] IV {
            get
            {
                if (_iv == null)
                {
                    _iv = GenerateRandomBytes(128);
                }

                return _iv;
            }
        }

        /// <summary>
        /// Creates AesCrypt object.
        /// </summary>
        /// <param name="secretKey">Secret key bytes array used by AES. If null is passed, it will be generated randomly. By default 256-bits key is generated.</param>
        /// <param name="initalizationVector">Initialization vector for AES algorithm (must be exact 128-bit (so 16 bytes) long). If null is passed, it will be generated randomly.</param>
        public AesCrypt(byte[] secretKey = null, byte[] initalizationVector = null)
        {
            _key = secretKey;
            _iv = initalizationVector;
        }

        /// <summary>
        /// Generates random byte array for specified bits number.
        /// </summary>
        /// <param name="bits">Number of bits for which random bytes array should be created.</param>
        /// <returns>Randomly generated byte array</returns>
        private static byte[] GenerateRandomBytes(int bits)
        {
            var result = new byte[bits / 8];
            RandomNumberGenerator.Create().GetBytes(result);
            return result;
        }

        /// <summary>
        /// Encrypts any string value with AES algorithm annd returns it's encrypted value coded in URL friendly Base64.
        /// </summary>
        /// <param name="stringifiedObject">String value which should be encrypted.</param>
        /// <returns>AES encrypted and Base64 transformed string.</returns>
        public string Encrypt(string stringifiedObject)
        {
            var inputBytes = Encoding.UTF8.GetBytes(stringifiedObject);
            var outputBytes = new byte[inputBytes.Length];

            using (var aes = Aes.Create())
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, aes.CreateEncryptor(Key, IV), CryptoStreamMode.Write))
                using (var inputStream = new MemoryStream(inputBytes))
                {
                    inputStream.CopyTo(cs);
                }

                outputBytes = ms.ToArray();
            }

            return ConvertToUrlFriendlyBase64(Convert.ToBase64String(outputBytes));
        }

        /// <summary>
        /// Decrypts any previously encrypted in AES + Base64 URL friendly string value.
        /// </summary>
        /// <param name="base64string">String value which should be decrypted.</param>
        /// <returns>Decrypted UTF8 string value.</returns>
        public string Decrypt(string base64string)
        {
            var cryptBytes = Convert.FromBase64String(ConvertFromUrlFriendlyBase64(base64string));
            byte[] clearBytes = null;

            using (var aes = Aes.Create())
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, aes.CreateDecryptor(Key, IV), CryptoStreamMode.Write))
                {
                    cs.Write(cryptBytes, 0, cryptBytes.Length);
                }

                clearBytes = ms.ToArray();
            }

            return Encoding.UTF8.GetString(clearBytes);
        }

        /// <summary>
        /// Converts Base64 string to URL friendly form (without '+' and '/' characters)
        /// </summary>
        /// <param name="input">Input Base64 coded string.</param>
        /// <returns>URL friendly Base64 string.</returns>
        private static string ConvertToUrlFriendlyBase64(string input)
        {
            return input.TrimEnd(padding).Replace('+', '-').Replace('/', '_');
        }

        /// <summary>
        /// Convert any previously URL friendly Base64 encoded string to normal Base64 string.
        /// </summary>
        /// <param name="input">Url friendly Base64 coded string.</param>
        /// <returns>Normal version of Base64 string.</returns>
        private static string ConvertFromUrlFriendlyBase64(string input)
        {
            string incoming = input.Replace('_', '/').Replace('-', '+');
            switch (incoming.Length % 4)
            {
                case 2: incoming += "=="; break;
                case 3: incoming += "="; break;
            }

            return incoming;
        }
    }
}