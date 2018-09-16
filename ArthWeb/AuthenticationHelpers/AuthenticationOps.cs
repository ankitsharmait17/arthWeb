using BE;
using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationHelpers
{
    public class AuthenticationOps
    {
        public bool AuthenticateUser(string username, string password, string clientIPAddress, string user_agent, string serverIPAddress, string urlRoute, string loginURL)
        {
            bool returnResult = false;
            //returnResult = new AuthBL(username, password, clientIPAddress, user_agent, serverIPAddress, urlRoute, loginURL).AuthenticateUser();
            return returnResult;
        }
        public bool LoginSuccessful(string username, string ipAddress, string user_agent)
        {
            var user = new UserBL().GetUser(username);
            //return new AuthBL().LoginSuccessful(user.UserID, ipAddress, user_agent);
        }
        public bool LoginFailed(string username, string clientIPAddress, string user_agent, string serverIPAddress, string urlRoute, string loginURL)
        {
            var user = new UserBL().GetUser(username);
            //return new AuthBL(null, null, clientIPAddress, user_agent, serverIPAddress, urlRoute, loginURL).LoginFailed(user.UserID);
        }

        public string GetUserData(string username)
        {
            var userData = new UserBL().GetUserData(username);
            return userData;
        }
    }
}
