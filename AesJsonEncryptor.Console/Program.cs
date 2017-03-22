using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace AesCrypt.DemoConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Provide any string:");
            var line = Console.ReadLine();

            var aesCrypt = new AesCrypt();
            var encryptedString = aesCrypt.Encrypt(line);
            var decryptedString = aesCrypt.Decrypt(encryptedString);

            Console.WriteLine("Encrypted value: ");
            Console.WriteLine();
            Console.WriteLine(encryptedString);
            Console.WriteLine("Decrypted value: ");
            Console.WriteLine();
            Console.WriteLine(decryptedString);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}