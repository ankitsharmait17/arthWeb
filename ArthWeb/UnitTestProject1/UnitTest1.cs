using System;
using DL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var x = new ItemTypeDAO().GetItemType();
        }

        [TestMethod]
        public void TestMethod2()
        {
            var x = new ItemDAO().GetItems();
        }

        [TestMethod]
        public void TestMethod3()
        {
            var x = new ItemMappingDAO().GetItemMappings();
        }

        [TestMethod]
        public void TestMethod4()
        {
            var x = new ItemDAO().GetItemsforGrid(null, 10, 0, "0", "Men|Women|", "Kurta|Kalidar|", null);
        }
    }
}
