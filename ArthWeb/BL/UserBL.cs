using BE;
using DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class UserBL
    {
        public bool ValidateUser(string email, string password)
        {
            return new UserDAO().ValidateUser(email,password);
        }

        public User GetUser(string usrname)
        {
            return new UserDAO().GetUser(usrname);
        }

        public string GetUserData(string username)
        {
            return new UserDAO().GetUserData(username);
        }

        public bool AddUser(User user)
        {
            return new UserDAO().AddUser(user);
        }

        public bool UpdateUser(User user)
        {
            return new UserDAO().UpdateUser(user);
        }
    }
}
