using BE;
using DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class AddressBL
    {
        public List<Address> GetAddressforUserID(int userID)
        {
            return new AddressDAO().GetAddressforUserID(userID);
        }

        public int AddAddress(Address address)
        {
            return new AddressDAO().AddAddress(address);
        }

        public bool UpdateAddress(Address address)
        {
            return new AddressDAO().UpdateAddress(address);
        }
    }
}
