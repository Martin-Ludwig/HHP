using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace HHP.Utils
{
    /// <summary>
    /// The main Cryptography class.
    /// Contains all methods for performing basic encryption functions.
    /// </summary>
    public class Crypto
    {
        /// <summary>
        /// Encrypt with SHA256 and hash a string.
        /// </summary>
        /// <returns> encrypted and hashed string </returns>
        public static string ComputeSHA256(string text)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(text));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }
    }
}
