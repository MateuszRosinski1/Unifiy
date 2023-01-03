using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Lista6
{
    public class PasswordHashMethods
    {
        public string GenerateSalt()
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            return Convert.ToBase64String(salt);
        }

        public byte[] GetHash(string password, string salt)
        {
            byte[] byteArray = Encoding.Unicode.GetBytes(String.Concat(salt, password));
            SHA256Managed sha256 = new SHA256Managed();
            byte[] hashedBytes = sha256.ComputeHash(byteArray);
            return hashedBytes;
        }

        public bool CompareHashedPasswords(string userInputPassword, string ExistingHashedPassword, string salt)
        {
            string UserInputttedHashedPassword = Convert.ToBase64String(GetHash(userInputPassword, salt));
            return ExistingHashedPassword == UserInputttedHashedPassword;
        }
    }
}
