using BE;
using Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE.Models;
using Newtonsoft.Json;

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

        public User GetUser(string usrname)
        {
            User usr = null;
            try
            {
                using (ArthModel cntx=new ArthModel())
                {
                    usr = cntx.Users.Where(x => x.EmailID.Equals(usrname)).FirstOrDefault();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return usr;
        }

        public string GetUserData(string username)
        {
            var user = GetUser(username);
            string userdata = JsonConvert.SerializeObject(new UserData()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone
            });
            return userdata;
        }
    }
}
