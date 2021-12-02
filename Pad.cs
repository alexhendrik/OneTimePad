using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OneTimePad
{
    internal class Pad : IDisposable
    {
        /// <summary>
        /// Byte array storage for the single use key.
        /// </summary>
        private byte[] TempKey { get; set; }

        /// <summary>
        /// Initializes a pad instance and populates the key using a pseudo-random function.
        /// </summary>
        /// <param name="keyLengthBytes">Desired key length in bytes.</param>
        public Pad(int keyLengthBytes)
        {
            TempKey = new byte[keyLengthBytes];

            using (var gen = RandomNumberGenerator.Create()) // Dispose of the RNG as soon as it populates the key
                gen.GetBytes(TempKey);

            Console.WriteLine($"Created a new OTP instance with key:\t{System.Convert.ToHexString(TempKey)}");
        }

        /// <summary>
        /// Performs the XOR conversion with the given input and the stored single use key.
        /// </summary>
        /// <param name="input">Either the plaintext or the ciphertext array.</param>
        /// <returns>Result of the XOR operation.</returns>
        public byte[] Convert(byte[] input)
        {
            var result = new byte[input.Length];

            if (input.Length != TempKey.Length)
                throw new ArgumentException("The input length does not match the length of the key!");

            for(int b = 0; b < input.Length; b++)
                result[b] = (byte)(input[b] ^ TempKey[b]);

            return result;
        }

        /// <summary>
        /// Provisional method for decrypting with a key from an external source, currently unused.
        /// </summary>
        /// <param name="ciphertext">The ciphertext array.</param>
        /// <param name="newKey">Externally sourced key.</param>
        /// <returns>Result of the XOR operation.</returns>
        public byte[] ConvertWithNewKey(byte[] ciphertext, byte[] newKey)
        {
            TempKey = newKey;

            return Convert(ciphertext);
        }

        /// <summary>
        /// Clean the memory locations of the single use key and run the garbage collector.
        /// </summary>
        public void Dispose()
        {
            for(int i = 0; i < TempKey.Length; i++)
                TempKey[i] = 0;

            GC.Collect();
            GC.SuppressFinalize(this);

            Console.WriteLine("OTP instance has been sanitized and disposed.");
        }
    }
}
