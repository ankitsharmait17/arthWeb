using BE;
using BE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    public class ItemDAO
    {
        public List<Item> GetItems()
        {
            List<Item> items = null;
            try
            {
                using (ArthModel cntx = new ArthModel())
                {
                    items = cntx.Items.ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return items;
        }

        public Item GetItem(string itemKey)
        {
            Item item = null;
            try
            {
                using (ArthModel cntx = new ArthModel())
                {
                    item = cntx.Items.Where(x => x.ItemKey.Equals(itemKey)).FirstOrDefault();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return item;
        }

        public List<ItemModel> GetItemsforGrid(string search, int pageSize, int startRec, string order,string filterGender,string filterSubtype,string filterPrice,string filterSize)
        {
            List<ItemModel> itemList = null;
            try
            {
                using (ArthModel cntx = new ArthModel())
                {
                    cntx.Configuration.LazyLoadingEnabled = false;
                    var data = (from item in cntx.Items
                                join itemMap in cntx.ItemMappings
                                on item.ItemMappingID equals itemMap.ItemMappingID
                                join type in cntx.ItemTypes
                                on itemMap.ItemTypeID equals type.ItemTypeID
                                join subType in cntx.ItemSubTypes
                                on itemMap.ItemSubTypeID equals subType.ItemSubTypeID
                                select new { item, itemMap, type, subType});
                    

                    if (!string.IsNullOrWhiteSpace(search))
                    {
                        data = data.Where(x => x.item.Description.Trim().ToLower().Contains(search.Trim().ToLower()) ||
                                             x.item.DescriptionLong.Trim().ToLower().Contains(search.Trim().ToLower()));
                    }

                    
                    if (!string.IsNullOrWhiteSpace(filterGender))
                    {
                        filterGender = filterGender.Remove(filterGender.Length-1);
                        var keywords = filterGender.Split('|');
                        string predicate = null;
                        int i;
                        for (i = 0; i < keywords.Length - 1; i++)
                        {
                            predicate += "itemMap.Gender.Equals(@" + i + ") ||";
                        }
                        predicate += "itemMap.Gender.Equals(@" + i + ")";
                        data = data.Where(predicate, keywords);
                    }

                    if (!string.IsNullOrWhiteSpace(filterPrice))
                    {
                        var keywords = filterPrice.Split('-');
                        decimal start = Convert.ToDecimal(keywords[0]);
                        decimal end = Convert.ToDecimal(keywords[1]);
                        data = data.Where(x=>x.item.Price>=start && x.item.Price<=end);
                    }

                    if (!string.IsNullOrWhiteSpace(filterSubtype))
                    {
                        filterSubtype = filterSubtype.Remove(filterSubtype.Length - 1);
                        var keywords = filterSubtype.Split('|');
                        string predicate = null;
                        int i;
                        for (i = 0; i < keywords.Length-1; i++)
                        {
                            predicate += "subType.ItemSubTypeKey.Contains(@" + i + ") || ";
                        }
                        predicate += "subType.ItemSubTypeKey.Contains(@" + i + ")";
                        data = data.Where(predicate, keywords);
                    }

                    if (!string.IsNullOrWhiteSpace(filterSize))
                    {
                        var sizeCheck = (from joined in data
                                         join qty in cntx.ItemQuantities
                                         on joined.item.ItemID equals qty.ItemID
                                         join itemSize in cntx.ItemSizes
                                         on qty.ItemSizeID equals itemSize.ItemSizeID
                                         select new { qty,itemSize });
                        filterSize = filterSize.Remove(filterSize.Length - 1);
                        var keywords = filterSize.Split('|');
                        string predicate = null;
                        int i;
                        for (i = 0; i < keywords.Length - 1; i++)
                        {
                            predicate += "itemSize.ItemSizeName.Equals(@" + i + ") || ";
                        }
                        predicate += "itemSize.ItemSizeName.Equals(@" + i + ")";
                        sizeCheck = sizeCheck.Where(predicate, keywords);
                        var distinctitems = sizeCheck.AsEnumerable().Select(x => x.qty.ItemID).Distinct();
                        data = (from joined in data
                                join item in distinctitems
                                on joined.item.ItemID equals item
                                select new { joined.item,joined.itemMap,joined.type,joined.subType });
                    }


                    switch (order)
                    {
                        case "0":
                            data = data.OrderByDescending(x => x.item.Price);
                            break;
                        case "1":
                            data = data.OrderBy(x => x.item.Price);
                            break;
                        case "2":
                            data = data.OrderBy(x => x.item.Description);
                            break;
                        case "3":
                            data = data.OrderByDescending(x => x.item.DescriptionLong);
                            break;
                        default:
                            data = data.OrderBy(x => x.item.ItemID);
                            break;
                    }

                    data = data.Skip(startRec).Take(pageSize);
                    itemList = data.AsEnumerable()
                                .Select(x => new ItemModel()
                                {
                                    ItemID = x.item.ItemID,
                                    ItemKey = x.item.ItemKey,
                                    Description = x.item.Description,
                                    DescriptionLong = x.item.DescriptionLong,
                                    Gender = x.itemMap.Gender,
                                    ItemSubType = x.subType.ItemSubTypeKey,
                                    ItemType = x.type.ItemTypeKey,
                                    Price = x.item.Price
                                }).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
            
            return itemList;
        }

        public List<ItemCartModel> GetCartItems(List<ItemCartModel> items,bool sizeCheck)
        {
            List<ItemCartModel> itemCarts= null;
            try
            {
                using(ArthModel cntx=new ArthModel())
                {
                    itemCarts = new List<ItemCartModel>();
                    foreach (ItemCartModel it in items)
                    {
                        var item = cntx.Items.FirstOrDefault(x => x.ItemKey.Equals(it.ItemKey));
                        var size = cntx.ItemSizes.FirstOrDefault(x => x.ItemSizeName.Equals(it.Size));
                        if (size != null)
                        {
                            if (sizeCheck)
                            {
                                var qty = cntx.ItemQuantities.FirstOrDefault(x => x.ItemID == item.ItemID && x.ItemSizeID == size.ItemSizeID);
                                if (qty != null)
                                {
                                    if (qty.Quantity < it.Quantity)
                                    {
                                        continue;
                                    }
                                }
                            }
                            if (item != null)
                            {
                                itemCarts.Add(new ItemCartModel()
                                {
                                    ItemKey = item.ItemKey,
                                    Description = item.Description,
                                    Quantity = it.Quantity,
                                    Size = it.Size,
                                    Price = item.Price
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return itemCarts;
        }

         
    }
}
