using BE;
using Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE.Models;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Configuration;

namespace DL
{
    public class UserDAO
    {
        public bool ValidateUser(string email,string password)
        {
            bool isCorrect=false;
            using (ArthModel cntx = new ArthModel())
            {
                var user = cntx.Users.Where(x => x.EmailID.Equals(email)).FirstOrDefault();
                if (user == null)
                    return false;
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
                Name = user.Name,
                Phone = user.Phone
            });
            return userdata;
        }

        public bool AddUser(User user,string url)
        {
            try
            {
                using (ArthModel cntx=new ArthModel())
                {
                    var isPresent = cntx.Users.Where(x => user.EmailID.Equals(x.EmailID)).FirstOrDefault();
                    if (isPresent != null)
                        return false;
                    user.Password = new Hash().GenerateHash(user.Password,new Salt().GenerateSalt());
                    user.Confirmation =  Guid.NewGuid();
                    var entity=cntx.Users.Add(user);
                    cntx.SaveChanges();
                    SendEmail(entity.Confirmation.ToString(), user.EmailID, user.Name,url);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return true;
        }

        public bool UpdateUser(User user)
        {
            int rows;
            try
            {
                using (ArthModel cntx=new ArthModel())
                {
                    var exUser = cntx.Users.Where(x => x.EmailID.Equals(user.EmailID)).FirstOrDefault();
                    if (exUser == null)
                        return false;
                    exUser.Name = user.Name;
                    exUser.Phone = user.Phone;
                    //cntx.Entry(exUser).State = System.Data.Entity.EntityState.Modified;
                    rows=cntx.SaveChanges();
                }
            }
            catch (Exception)
            {

                return false;
            }
            if (rows > 0)
                return true;
            else
                return false;
        }

        public void SendEmail(string actCode,string email,string name,string url)
        {
            url += "?email=" + email + "&code=" + actCode;
            using (MailMessage mail=new MailMessage())
            {
                mail.From = new MailAddress(ConfigurationManager.AppSettings["SenderMailID"], "Arth Support");
                mail.To.Add(new MailAddress(email,name));
                mail.Subject = "Activation Email :Please activate your profile";
                string body = "Hello " + name + ",";
                body += "<br /><br />Please click the following link to activate your account";
                body += "<br /><a href = '" + url + "'>Click here to activate your account.</a>";
                body += "<br /><br />Thanks";
                mail.Body = body;
                mail.IsBodyHtml = true;
                using (SmtpClient MailClient = new SmtpClient(ConfigurationManager.AppSettings["Host"], Convert.ToInt32(ConfigurationManager.AppSettings["SenderMailPort"])))
                {
                    MailClient.EnableSsl = true;
                    MailClient.UseDefaultCredentials = false;
                    MailClient.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SenderMailID"], ConfigurationManager.AppSettings["SenderMailPassword"]);
                    MailClient.Send(mail);
                }
            }
        }

        public int ConfirmEmail(string email,string confirmationCode)
        {
            bool isSuccess = false;
            try
            {
                var checkEmail = GetUser(email);
                if (checkEmail == null)
                    return 1;
                if (checkEmail != null && checkEmail.IsActive == true)
                    return 2;
                if (checkEmail.Confirmation.ToString() == confirmationCode)
                    isSuccess=ActivateUser(email);
            }
            catch (Exception ex)
            {

                throw;
            }
            return isSuccess?0:3;
        }

        public bool ActivateUser(string username)
        {
            bool isSuccess = false;
            try
            {
                using (ArthModel cntx = new ArthModel())
                {
                    var exUser = cntx.Users.Where(x => x.EmailID.Equals(username)).FirstOrDefault();
                    if (exUser == null)
                        return false;
                    exUser.IsActive =true;
                    cntx.Entry(exUser).State = System.Data.Entity.EntityState.Modified;
                    var rows = cntx.SaveChanges();
                    isSuccess = rows > 0 ? true:false;
                }
            }
            catch (Exception)
            {

                return false;
            }
            return isSuccess;
        }

        public int ChangePassword(string username,string oldPassword,string newPassword)
        {
            bool isSuccess =false;
            try
            {
                var validate = ValidateUser(username, oldPassword);
                if (!validate)
                    return 1;
                isSuccess = ChangePasswordDB(username, newPassword);
            }
            catch (Exception)
            {

                throw;
            }
            return isSuccess ? 0 : 2;
        }

        public bool ChangePasswordDB(string username,string password)
        {
            bool isSuccess = false;
            try
            {
                var hashPass= new Hash().GenerateHash(password, new Salt().GenerateSalt());
                using (ArthModel cntx = new ArthModel())
                {
                    var exUser = cntx.Users.Where(x => x.EmailID.Equals(username)).FirstOrDefault();
                    if (exUser == null)
                        return false;
                    exUser.Password = hashPass;
                    //cntx.Entry(exUser).State = System.Data.Entity.EntityState.Modified;
                    var rows = cntx.SaveChanges();
                    isSuccess = rows > 0 ? true : false;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return isSuccess;
        }
    }
}
