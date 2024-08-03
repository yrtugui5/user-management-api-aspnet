using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace P2Api
{
    public static class Settings
    {
        public static string Secret = "NL$4PCiW8lBh_c0uz#gv#K8";

        // public static string ApiKeyName = "api_Key";
        // public static string ApiKey = "sd55wds_shdgjfhaf=l2wwwdsd";

        public static string GenerateHash(string password)
        {
            byte[] salt = Encoding.ASCII.GetBytes("151654562215");
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 32
                ));
            return hashed;
        }
    }
}
