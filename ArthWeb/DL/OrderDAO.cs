using BE;
using BE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    public class OrderDAO
    {
        public Dictionary<string, string> PlaceOrder(int addressID,string userID,List<ItemCartModel> orderDetails)
        {
            //bool isSuccess = false;
            Dictionary<string, string> exList = new Dictionary<string, string>();
            try
            {
                using (ArthModel cntx=new ArthModel())
                {
                    var address = cntx.Addresses.FirstOrDefault(x => x.AddressID == addressID);
                    if (address == null)
                        throw new Exception("Address does not exist.");
                    var user = cntx.Users.FirstOrDefault(x => x.EmailID.Equals(userID));
                    if(user==null)
                        throw new Exception("User does not exist.");
                    if(address.UserID!=user.UserID)
                        throw new Exception("Address does not belong to user.");
                    var addedOrder = cntx.Orders.Add(new Order()
                    {
                        UserID = user.UserID,
                        OrderDate = DateTime.UtcNow,
                        Delivered = false,
                        Status = "Placed",
                        Name = address.Name,
                        AddressDetail = address.AddressDetail,
                        AltPhone = address.AltPhone,
                        City = address.City,
                        Landmark = address.Landmark,
                        Phone = address.Phone,
                        Pin = address.Pin,
                        State = address.State,
                        Type = address.Type,
                        OrderDeliveryDate = DateTime.UtcNow.AddDays(5)
                    });
                    cntx.SaveChanges();
                    exList.Add("OrderID", addedOrder.OrderID.ToString());
                    foreach (ItemCartModel item in orderDetails)
                    {
                        var addedOrderDetail = cntx.OrderDetails.Add(new OrderDetail()
                        {
                            ItemKey=item.ItemKey,
                            ItemQuantity=item.Quantity,
                            ItemSize=item.Size,
                            PricePerItem=item.Price,
                            OrderID=addedOrder.OrderID
                        });
                        var it = cntx.Items.FirstOrDefault(x => x.ItemKey.Equals(item.ItemKey));
                        var size = cntx.ItemSizes.FirstOrDefault(x => x.ItemSizeName.Equals(item.Size));
                        if (it != null && size!=null)
                        {
                            var quantity = cntx.ItemQuantities.FirstOrDefault(x => x.ItemID == it.ItemID && size.ItemSizeID==x.ItemSizeID);
                            if (quantity != null)
                            {
                                try
                                {
                                    quantity.Quantity -= item.Quantity;
                                    if (quantity.Quantity < 0)
                                        throw new Exception("Quantity is lesser in stock for " + it.Description);
                                    cntx.Entry(quantity).State = System.Data.Entity.EntityState.Modified;
                                }
                                catch(Exception ex)
                                {
                                    exList.Add(it.ItemKey, ex.Message);
                                }
                            }   
                        }
                    }
                    var rows=cntx.SaveChanges();
                    //isSuccess = rows > 0;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return exList;
        }

        public List<OrderModel> GetOrdersforUser(int userID)
        {
            List<OrderModel> orders = null;
            try
            {
                using (ArthModel cntx=new ArthModel())
                {
                    var data = (from ord in cntx.Orders
                                where ord.UserID == userID
                                join usr in cntx.Users
                                on ord.UserID equals usr.UserID
                                select new { ord, usr }).AsEnumerable()
                              .Select(x => new OrderModel()
                              {
                                  Address = new Address()
                                  {
                                      AddressDetail = x.ord.AddressDetail,
                                      AltPhone = x.ord.AltPhone,
                                      City = x.ord.City,
                                      Landmark = x.ord.Landmark,
                                      Name = x.ord.Name,
                                      Phone = x.ord.Phone,
                                      Pin = x.ord.Pin,
                                      State = x.ord.State,
                                      Type = x.ord.Type,
                                      UserID = x.ord.UserID
                                  },
                                  UserID = x.ord.UserID,
                                  Delivered = x.ord.Delivered,
                                  OrderDate = x.ord.OrderDate,
                                  OrderDeliveredDate = x.ord.OrderDeliveredDate,
                                  OrderDeliveryDate = x.ord.OrderDeliveryDate,
                                  OrderID = x.ord.OrderID,
                                  Status = x.ord.Status,
                                  Username = x.usr.EmailID
                              });
                    orders = data.ToList();
                    
                }
            }
            catch (Exception)
            {

                throw;
            }
            return orders;
        }

        public OrderModel GetOrder(int orderID,int userID)
        {
            OrderModel order = null;
            try
            {
                using (ArthModel cntx=new ArthModel())
                {
                    order = (from ord in cntx.Orders
                             where ord.OrderID == orderID && ord.UserID==userID
                             select new { ord }).AsEnumerable()
                           .Select(x => new OrderModel()
                           {
                               Address = new Address()
                               {
                                   AddressDetail = x.ord.AddressDetail,
                                   AltPhone = x.ord.AltPhone,
                                   City = x.ord.City,
                                   Landmark = x.ord.Landmark,
                                   Name = x.ord.Name,
                                   Phone = x.ord.Phone,
                                   Pin = x.ord.Pin,
                                   State = x.ord.State,
                                   Type = x.ord.Type,
                                   UserID = x.ord.UserID
                               },
                               Status = x.ord.Status,
                               OrderID = x.ord.OrderID,
                               Delivered = x.ord.Delivered,
                               OrderDate = x.ord.OrderDate,
                               OrderDeliveredDate = x.ord.OrderDeliveredDate,
                               OrderDeliveryDate = x.ord.OrderDeliveryDate,
                               UserID = x.ord.UserID
                           }).FirstOrDefault();
                    
                }
            }
            catch (Exception)
            {

                throw;
            }
            return order;
        }

        public bool ChangeOrderStatus(int orderID,int userID,string status)
        {
            bool isSuccess = false;
            try
            {
                using (ArthModel cntx = new ArthModel())
                {
                    var order = cntx.Orders.FirstOrDefault(x => x.OrderID == orderID && x.UserID == userID);
                    if (order == null)
                        return false;
                    order.Status = status;
                    isSuccess=cntx.SaveChanges()>0;
                }
            }
            catch(Exception)
            {
                return false;
            }
            return isSuccess;
        }
    }
}
