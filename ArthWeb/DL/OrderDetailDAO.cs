using BE;
using BE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    public class OrderDetailDAO
    {
        public List<OrderDetailModel> GetOrderDetailsforOrder(int orderID)
        {
            List<OrderDetailModel> orderdetails = null;
            try
            {
                using (ArthModel cntx=new ArthModel())
                {
                    var data = (from det in cntx.OrderDetails where det.OrderID==orderID
                                join item in cntx.Items
                                on det.ItemKey equals item.ItemKey
                                select new { det, item }).AsEnumerable()
                              .Select(x => new OrderDetailModel()
                              {
                                  OrderDetailID=x.det.OrderDetailID,
                                  ItemDesc=x.item.DescriptionLong,
                                  ItemKey=x.item.ItemKey,
                                  PricePerItem=x.det.PricePerItem,
                                  Quantity=x.det.ItemQuantity,
                                  Size=x.det.ItemSize
                              });
                    orderdetails = data.ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return orderdetails;
        }
    }
}
