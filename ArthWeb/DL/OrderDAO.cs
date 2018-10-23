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
                        AddressID = addressID,
                        OrderDate = DateTime.UtcNow,
                        Delivered = false
                    });
                    cntx.SaveChanges();
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
                        if (it != null)
                        {
                            var quantity = cntx.ItemQuantities.FirstOrDefault(x => x.ItemID == it.ItemID);
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
    }
}
