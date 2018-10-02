using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    public class AddressDAO
    {
        public List<Address> GetAddressforUserID(int userID)
        {
            List<Address> addresses = null;
            try
            {
                using (ArthModel cntx=new ArthModel())
                {
                    addresses = cntx.Addresses.Where(x => x.UserID == userID).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return addresses;
        }

        public int AddAddress(Address address)
        {
            int addedId;
            try
            {
                using (ArthModel cntx=new ArthModel())
                {
                    var entity=cntx.Addresses.Add(address);
                    var rows=cntx.SaveChanges();
                    addedId = entity != null ? entity.AddressID : 0;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return addedId;
        }

        public bool UpdateAddress(Address address)
        {
            bool isSuccess = false;
            try
            {
                using (ArthModel cntx=new ArthModel())
                {
                    var exAddress = cntx.Addresses.Where(x => x.AddressID == address.AddressID).FirstOrDefault();
                    if (exAddress == null)
                        return false;
                    exAddress.AddressDetail = address.AddressDetail;
                    exAddress.AltPhone = address.AltPhone;
                    exAddress.City = address.City;
                    exAddress.Landmark = address.Landmark;
                    exAddress.Name = address.Name;
                    exAddress.Phone = address.Phone;
                    exAddress.Pin = address.Pin;
                    exAddress.State = address.State;
                    exAddress.Type = address.Type;
                    cntx.Entry(exAddress).State = System.Data.Entity.EntityState.Modified;
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

        public bool DeleteAddress(int addID)
        {
            bool isSuccess = false;
            try
            {
                using (ArthModel cntx=new ArthModel())
                {
                    var exAdd = cntx.Addresses.Where(x => x.AddressID == addID).FirstOrDefault();
                    if (exAdd == null)
                        return false;
                    cntx.Addresses.Remove(exAdd);
                    var rows=cntx.SaveChanges();
                    isSuccess = rows > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return true;
        }
    }
}
