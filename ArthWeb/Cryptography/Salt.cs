using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Cryptography
{
    class Salt
    {
        int saltLength;
        public Salt()
        {
            saltLength = Convert.ToInt32(ConfigurationManager.AppSettings["HASH_SALT_SIZE"]);
        }
        public byte[] GenerateSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] buffer = new byte[saltLength];
                rng.GetBytes(buffer);
                return buffer;

            }
        }
        public byte[] GetSalt(string saltRepresentation)
        {
            return Convert.FromBase64String(saltRepresentation);
        }
    }
}
