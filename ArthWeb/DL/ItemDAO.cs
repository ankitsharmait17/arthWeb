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
            using (ArthModel cntx=new ArthModel())
            {
                items = cntx.Items.ToList();
            }
            return items;
        }

        public List<ItemModel> GetItemsforGrid(string search, int pageSize, int startRec, string order,string filterGender,string filterSubtype,string filterPrice)
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
                                select new { item, itemMap, type, subType });
                    

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
    }
}
