using BE;
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

        public List<OrderModel> GetOrdersforUser(int userID)
        {
            var orders = new OrderDAO().GetOrdersforUser(userID);
            var orderDetailBL = new OrderDetailBL();
            foreach(var order in orders)
            {
                order.OrderDetails = orderDetailBL.GetOrderDetailsforOrder(order.OrderID);
            }
            return orders;
        }
    }
}
