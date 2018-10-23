using BE.Models;
using DL;
using System.Collections.Generic;

namespace BL
{
    public class OrderBL
    {
        public Dictionary<string,string> PlaceOrder(int addressID, string userID, List<ItemCartModel> orderDetails)
        {
            return new OrderDAO().PlaceOrder(addressID, userID, orderDetails);
        }
    }
}
