using System;
using System.Linq;
using System.Text;

namespace OneTimePad
{
    public class Program
    {
        /// <summary>
        /// Application entry point.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            ProgramLoop();
        }

        /// <summary>
        /// Primary program execution loop.
        /// </summary>
        private static void ProgramLoop()
        {
            Console.WriteLine("Welcome to TSOTP: Totally Secure One Time Pad\nBy Alex Hendrik for CS 538");

            while (true)
            {
                var plaintextBytes = Encoding.UTF8.GetBytes(GatherUserInput());

                Console.WriteLine("\n-----\n");

                using (var pad = new Pad(plaintextBytes.Length))
                {
                    var ciphertext = pad.Convert(plaintextBytes);

                    Console.WriteLine($"Hex Plaintext:\t\t\t\t{Convert.ToHexString(plaintextBytes)}");
                    Console.WriteLine($"Ciphertext:\t\t\t\t{Convert.ToHexString(ciphertext)}");
                    Console.WriteLine($"Test Decryption:\t\t\t{Encoding.UTF8.GetString(pad.Convert(ciphertext))}");
                }

                Console.WriteLine("\n#####\n");
            }
        }

        /// <summary>
        /// Repeatedly attempt to gather plaintext, verify, and sanitize it.
        /// </summary>
        /// <returns>Sanitized plaintext.</returns>
        private static string GatherUserInput()
        {
            Console.WriteLine("\nPlease enter the desired plaintext or use Ctrl + C to exit:");
            var unsafeIn = Console.ReadLine();

            if(string.IsNullOrEmpty(unsafeIn) || unsafeIn.Any(c => !char.IsLetterOrDigit(c)))
            {
                Console.WriteLine("Invalid plaintext provided, please make sure you avoid special characters!");
                unsafeIn = GatherUserInput();
            }

            return unsafeIn.Trim().ToLower();
        }
    }
}
