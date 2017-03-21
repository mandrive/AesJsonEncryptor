using AesJsonEncryptor.Console.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AesJsonEncryptor.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var aesJsonEncryptor = new AesJsonEncryptor();
            var groupsArray = new GroupsArray()
            {
                Groups = new int[2] { 1, 2 }
            };

            var encryptedString = aesJsonEncryptor.Encrypt(JsonConvert.SerializeObject(
                groupsArray,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }
            ));

            var decryptedString = aesJsonEncryptor.Decrypt(encryptedString);

            var deserializedGroupsArray = JsonConvert.DeserializeObject<GroupsArray>(decryptedString,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

            System.Console.WriteLine(encryptedString);
            System.Console.WriteLine(decryptedString);

            System.Console.ReadKey();
        }
    }
}