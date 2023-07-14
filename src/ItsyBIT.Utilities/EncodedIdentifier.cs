using CSharpVitamins;
using System.Configuration;
using System.Text;

namespace ItsyBIT.Utilities
{
    public class EncodedIdentifier
    {
        private Guid? guid;
        private string? shortKey { get; }

        private static string DefaultKey = "ItsyBITSecretKey";
        private static string? key = null;
        public static Func<string> GetKey = () => {
            key ??= ConfigurationManager.AppSettings["MyGuid"] ?? DefaultKey;
            return key;
        };
      
        public string EncodedShortKey => EncodeString(ShortKey, GetKey());

        private string ShortKey => shortKey != null ? shortKey : $"{new ShortGuid(guid.Value)}";
        public Guid Guid => guid.HasValue ? guid.Value : new ShortGuid(ShortKey).Guid; 
        public EncodedIdentifier(Guid guid)
        {
            this.guid = guid;

        }
        
        public EncodedIdentifier(string encodedShortGuid)
        {
            shortKey = DecodeString(encodedShortGuid, GetKey());
        }

        public static string EncodeString(string plainText, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] cipherTextBytes = new byte[plainTextBytes.Length];

            for (int i = 0; i < plainTextBytes.Length; i++)
            {
                cipherTextBytes[i] = (byte)(plainTextBytes[i] ^ keyBytes[i % keyBytes.Length]);
            }

            return Convert.ToBase64String(cipherTextBytes);
        }

        public static string DecodeString(string cipherText, string key)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            for (int i = 0; i < cipherTextBytes.Length; i++)
            {
                plainTextBytes[i] = (byte)(cipherTextBytes[i] ^ keyBytes[i % keyBytes.Length]);
            }

            var result = Encoding.UTF8.GetString(plainTextBytes);
            return result;
        }


    }
}
