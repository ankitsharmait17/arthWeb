using BE;
using Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    public class UserDAO
    {
        public bool ValidateUser(string email,string password)
        {
            bool isCorrect=false;
            using (ArthModel cntx = new ArthModel())
            {
                var user = cntx.Users.Where(x => x.IsActive && x.EmailID.Equals(email)).FirstOrDefault();
                isCorrect = new Hash().VerifyPassword(password, user.Password);
            }
            return isCorrect;
        }
    }
}
