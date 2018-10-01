using BE;
using BE.Models;
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

        public UserAddressModel GetUserWithAdresses(string username)
        {
            UserAddressModel user = null;
            var data = GetUser(username);
            user = new UserAddressModel()
            {
                Email = data.EmailID,
                Name = data.Name,
                Phone = data.Phone,
                Addresses = new AddressBL().GetAddressforUserID(data.UserID)
            };
            return user;
        }
    }
}
