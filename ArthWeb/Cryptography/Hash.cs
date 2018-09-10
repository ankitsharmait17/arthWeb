using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Cryptography
{
    public class Hash
    {
        int IterationCount;
        int DerivedKeyLength;
        int SaltSize;
        public Hash()
        {
            IterationCount = Convert.ToInt32(ConfigurationManager.AppSettings["HASH_ITERATION_COUNT"]);
            DerivedKeyLength = Convert.ToInt32(ConfigurationManager.AppSettings["HASH_DERIVED_KEY_LENGTH"]);
            SaltSize = Convert.ToInt32(ConfigurationManager.AppSettings["HASH_SALT_SIZE"]);
        }
        public string GenerateHash(string inputPassword, byte[] salt)
        {
            var hashValue = GenerateHashValue(inputPassword, salt, IterationCount);
            var iterationCountBtyeArr = BitConverter.GetBytes(IterationCount);
            var valueToSave = new byte[SaltSize + DerivedKeyLength + iterationCountBtyeArr.Length];
            Buffer.BlockCopy(salt, 0, valueToSave, 0, SaltSize);
            Buffer.BlockCopy(hashValue, 0, valueToSave, SaltSize, DerivedKeyLength);
            Buffer.BlockCopy(iterationCountBtyeArr, 0, valueToSave, salt.Length + hashValue.Length, iterationCountBtyeArr.Length);
            return Convert.ToBase64String(valueToSave);
        }


        private byte[] GenerateHashValue(string password, byte[] salt, int iterationCount)
        {
            byte[] hashValue;
            var valueToHash = string.IsNullOrEmpty(password) ? string.Empty : password;
            using (var pbkdf2 = new Rfc2898DeriveBytes(valueToHash, salt, iterationCount))
            {
                hashValue = pbkdf2.GetBytes(DerivedKeyLength);
            }
            return hashValue;
        }

        public bool VerifyPassword(string passwordGuess, string actualSavedHashResults)
        {
            //ingredient #1: password salt byte array
            var salt = new byte[SaltSize];

            //ingredient #2: byte array of password
            var actualPasswordByteArr = new byte[DerivedKeyLength];

            //convert actualSavedHashResults to byte array
            var actualSavedHashResultsBtyeArr = Convert.FromBase64String(actualSavedHashResults);

            //ingredient #3: iteration count
            var iterationCountLength = actualSavedHashResultsBtyeArr.Length - (salt.Length + actualPasswordByteArr.Length);
            var iterationCountByteArr = new byte[iterationCountLength];
            Buffer.BlockCopy(actualSavedHashResultsBtyeArr, 0, salt, 0, SaltSize);
            Buffer.BlockCopy(actualSavedHashResultsBtyeArr, SaltSize, actualPasswordByteArr, 0, actualPasswordByteArr.Length);
            Buffer.BlockCopy(actualSavedHashResultsBtyeArr, (salt.Length + actualPasswordByteArr.Length), iterationCountByteArr, 0, iterationCountLength);
            var passwordGuessByteArr = GenerateHashValue(passwordGuess, salt, BitConverter.ToInt32(iterationCountByteArr, 0));
            return ConstantTimeComparison(passwordGuessByteArr, actualPasswordByteArr);
        }
        private static bool ConstantTimeComparison(byte[] passwordGuess, byte[] actualPassword)
        {
            uint difference = (uint)passwordGuess.Length ^ (uint)actualPassword.Length;
            for (var i = 0; i < passwordGuess.Length && i < actualPassword.Length; i++)
            {
                difference |= (uint)(passwordGuess[i] ^ actualPassword[i]);
            }

            return difference == 0;
        }
    }
}
