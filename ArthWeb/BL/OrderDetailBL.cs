using BE.Models;
using DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class OrderDetailBL
    {
        public List<OrderDetailModel> GetOrderDetailsforOrder(int orderID)
        {
            return new OrderDetailDAO().GetOrderDetailsforOrder(orderID);
        }
    }
}
