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

        public bool AddUser(User user,string url)
        {
            return new UserDAO().AddUser(user,url);
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
                Addresses = new AddressBL().GetAddressforUserID(data.UserID),
                Orders=new OrderBL().GetOrdersforUser(data.UserID)
            };
            return user;
        }

        public int ConfirmEmail(string email, string confirmationCode)
        {
            return new UserDAO().ConfirmEmail(email, confirmationCode);
        }

        public int ChangePassword(string username, string oldPassword, string newPassword)
        {
            return new UserDAO().ChangePassword(username, oldPassword, newPassword);
        }

        public string ForgotPasswordEnable(string username)
        {
            return new UserDAO().ForgotPasswordEnable(username);
        }

        public bool ConfirmForgotPassword(string code, string email)
        {
            return new UserDAO().ConfirmForgotPassword(code, email);
        }

        public bool ResetPassword(string email,string password)
        {
            return new UserDAO().ResetPassword(email, password);
        }
    }
}
