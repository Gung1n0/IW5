using IW5_project.API.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IW5_project.Api.DAL.Common.Queries
{
    public class OrderItems
    {
        //order items by ascending order by name
        public List<ItemDTO> OrderByNameAscending(List<ItemDTO> Item)
        {
            if (Item != null && Item.Count > 1)
            {
                var arr = Item.ToArray();
                Array.Sort(arr, (a, b) => a.Name.CompareTo(b.Name));
                return arr.ToList();
            }
            List<ItemDTO> list = [];
            return list;
        }

        //order items by descending order by name
        public List<ItemDTO> OrderByNameDescending(List<ItemDTO> Item)
        {
            if (Item != null && Item.Count > 1)
            {
                var arr = OrderByNameAscending(Item).ToArray();
                Array.Reverse(arr);
                return arr.ToList();
            }
            List<ItemDTO> list = [];
            return list;
        }

        //fill in when place exists
        /*public List<ItemDTO> OrderByPlaceAscending(List<ItemDTO> Item, PlaceDTO Place)
        {
            if (Item != null && Item.Count > 1)
            {
                var arr = Item.ToArray();
                Array.Sort(arr, (a, b) => a.Place.PlaceId.CompareTo(b.Place.PlaceId));
                return arr.ToList();
            }
            return Item;
        }*/

        /*public List<ItemDTO> OrderByPlaceDescending(List<ItemDTO> Item)
        {
            if (Item != null && Item.Count > 1)
            {
                var arr = OrderByPlaceAscending(Item);
                int j = arr.Count - 1;
                if (arr.Count % 2 == 1)
                {
                    for (int i = 0; i < (arr.Count / 2) + 1; i++)
                    {
                        ItemDTO temp = arr[i];
                        arr[i] = arr[j];
                        arr[j] = temp;
                    }
                    return arr;
                }
                else
                {
                    for (int i = 0; i < arr.Count / 2; i++)
                    {
                        ItemDTO temp = arr[i];
                        arr[i] = arr[j];
                        arr[j] = temp;
                    }
                    return arr;
                }
            }
            return Item;
        }*/

        //do the same for ordering by users, fill in when user exists

        //order by amount ascending
        public List<ItemDTO> OrderByAmountAscending(List<ItemDTO> Item)
        {
            if (Item != null && Item.Count > 1)
            {
                var arr = Item.ToArray();
                Array.Sort(arr, (a, b) => a.Amount.CompareTo(b.Amount));
                return arr.ToList();
            }
            List<ItemDTO> list = [];
            return list;
        }

        //order by amount descending
        public List<ItemDTO> OrderByAmountDescending(List<ItemDTO> Item)
        {
            if (Item != null && Item.Count > 1)
            {
                var arr = OrderByAmountAscending(Item).ToArray();
                Array.Reverse(arr);
                return arr.ToList();
            }
            List<ItemDTO> list = [];
            return list;
        }
    }
}