using BE;
using BE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
                                select new { item,itemMap,type,subType }).AsEnumerable()
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
                                }).AsQueryable();
                    

                    if (!string.IsNullOrWhiteSpace(search))
                    {
                        data = data.Where(x => x.Description.Trim().ToLower().Contains(search.Trim().ToLower()) ||
                                             x.DescriptionLong.Trim().ToLower().Contains(search.Trim().ToLower()));
                    }

                    var predicate = PredicateBuilder.False<ItemModel>();
                    if (!string.IsNullOrWhiteSpace(filterGender))
                    {
                        var keywords = filterGender.Split('|');
                        for (int i = 0; i < keywords.Length - 1; i++)
                        {
                            string temp = keywords[i] == "men" ? "M" : "F";
                            predicate = predicate.Or(p => p.Gender.Equals(temp));
                        }
                        data = data.Where(predicate);
                    }

                    if (!string.IsNullOrWhiteSpace(filterPrice))
                    {

                    }

                    if (!string.IsNullOrWhiteSpace(filterSubtype))
                    {
                        var keywords = filterSubtype.Split('|');
                        for (int i = 0; i < keywords.Length - 1; i++)
                        {
                            string temp = keywords[i];
                            predicate = predicate.Or(p => p.ItemSubType.Trim().ToLower().Contains(temp.Trim().ToLower()));
                        }
                        data = data.Where(predicate);
                    }

                    
                    switch (order)
                    {
                        case "0":
                            data = data.OrderByDescending(x => x.Price);
                            break;
                        case "1":
                            data = data.OrderBy(x => x.Price);
                            break;
                        case "2":
                            data = data.OrderBy(x => x.Description);
                            break;
                        case "3":
                            data = data.OrderByDescending(x => x.DescriptionLong);
                            break;
                        default:
                            data = data.OrderBy(x => x.ItemID);
                            break;
                    }

                    data = data.Skip(startRec).Take(pageSize);
                    itemList = data.ToList();
                    //itemList = data.AsEnumerable().Select(x=>new ItemModel()
                    //{
                    //     ItemID=x.item.ItemID,
                    //     ItemKey=x.item.ItemKey,
                    //     Description=x.item.Description,
                    //     DescriptionLong=x.item.DescriptionLong,
                    //     Gender=x.itemMap.Gender,
                    //     ItemSubType=x.subType.ItemSubTypeKey,
                    //     ItemType=x.type.ItemTypeKey,
                    //     Price=x.item.Price
                    //}).ToList();
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
