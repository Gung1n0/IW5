using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    

    public class ItemFinder
    {
        public List<PolozkaDTO> FindItem(string target, List<PolozkaDTO> list)
        {
            if (string.IsNullOrEmpty(target))
                return new List<PolozkaDTO>();

            return list.Where(item => item.Name.ToLower().Contains(target.ToLower())).ToList();
        }

        public List<int> FindItemIds(string target, List<PolozkaDTO> list)
        {
            return FindItem(target, list).Select(item => item.Id).ToList();
        }
    }

    public class OrderItems
    {
        //order items by ascending order by name
        public List<PolozkaDTO> OrderByNameAscending(List<PolozkaDTO> Polozka)
        {
            if (Polozka != null && Polozka.Count > 1)
            {
                var arr = Polozka.ToArray();
                Array.Sort(arr, (a, b) => a.Name.CompareTo(b.Name));
                return arr.ToList();
            }
            return Polozka;
        }

        //order items by descending order by name
        public List<PolozkaDTO> OrderByNameDescending(List<PolozkaDTO> Polozka)
        {
            if (Polozka != null && Polozka.Count > 1)
            {
                var arr = OrderByNameAscending(Polozka).ToArray();
                Array.Reverse(arr);
                return arr.ToList();
            }
            return Polozka;
        }

        //fill in when place exists
        /*public List<PolozkaDTO> OrderByPlaceAscending(List<PolozkaDTO> Polozka, PlaceDTO Place)
        {
            if (Polozka != null && Polozka.Count > 1)
            {
                var arr = Polozka.ToArray();
                Array.Sort(arr, (a, b) => a.Place.PlaceId.CompareTo(b.Place.PlaceId));
                return arr.ToList();
            }
            return Polozka;
        }*/

        /*public List<PolozkaDTO> OrderByPlaceDescending(List<PolozkaDTO> Polozka)
        {
            if (Polozka != null && Polozka.Count > 1)
            {
                var arr = OrderByPlaceAscending(Polozka);
                int j = arr.Count - 1;
                if (arr.Count % 2 == 1)
                {
                    for (int i = 0; i < (arr.Count / 2) + 1; i++)
                    {
                        PolozkaDTO temp = arr[i];
                        arr[i] = arr[j];
                        arr[j] = temp;
                    }
                    return arr;
                }
                else
                {
                    for (int i = 0; i < arr.Count / 2; i++)
                    {
                        PolozkaDTO temp = arr[i];
                        arr[i] = arr[j];
                        arr[j] = temp;
                    }
                    return arr;
                }
            }
            return Polozka;
        }*/

        //do the same for ordering by users, fill in when user exists

        //order by amount ascending
        public List<PolozkaDTO> OrderByAmountAscending(List<PolozkaDTO> Polozka)
        {
            if (Polozka != null && Polozka.Count > 1)
            {
                var arr = Polozka.ToArray();
                Array.Sort(arr, (a, b) => a.Amount.CompareTo(b.Amount));
                return arr.ToList();
            }
            return Polozka;
        }

        //order by amount descending
        public List<PolozkaDTO> OrderByAmountDescending(List<PolozkaDTO> Polozka)
        {
            if (Polozka != null && Polozka.Count > 1)
            {
                var arr = OrderByAmountAscending(Polozka).ToArray();
                Array.Reverse(arr);
                return arr.ToList();
            }
            return Polozka;
        }
    }

    public class FilterItems
    {
        public enum Operator
        {
            LessThan,          // <
            GreaterThan,       // >
            LessThanOrEqual,   // <=
            GreaterThanOrEqual // >=
        };

        //Filter items by Category
        public List<PolozkaDTO> FilterByCategory(List<PolozkaDTO> Polozka, TypeOfCategory Category)
        {
            if (Polozka != null)
            {
                for (int i = Polozka.Count - 1; i >= 0; i--)
                {
                    if (Polozka[i].Category != Category)
                        Polozka.RemoveAt(i);
                }
                return Polozka;
            }
            return Polozka;
        }

        //Filter items by place
        /*public List<PolozkaDTO> FilterByPlace(List<PolozkaDTO> Polozka, Place place)
        {
            if (Polozka != null)
            {
                var arr = Polozka;
                foreach (var item in arr)
                {
                    if (item.place != place)
                        arr.Remove(item);
                }
                return arr;
            }
            return Polozka;
        }*/

        //Do the same but for user, wait for user structure

        //Filter items by amount
        public List<PolozkaDTO> FilterByAmount(List<PolozkaDTO> Polozka, float Amount, Operator op)
        {
            if (Polozka != null)
            {
                var arr = Polozka;
                for (int i = arr.Count() - 1; i >= 0; i--)
                {
                    if (Operator.LessThan == op && arr[i].Amount < Amount)
                        arr.RemoveAt(i);
                    else if (Operator.GreaterThan == op && arr[i].Amount > Amount)
                        arr.RemoveAt(i);
                    else if (Operator.LessThanOrEqual == op && arr[i].Amount <= Amount)
                        arr.RemoveAt(i);
                    else if (Operator.GreaterThanOrEqual == op && arr[i].Amount >= Amount)
                        arr.RemoveAt(i);
                }
                return arr;
            }
            return Polozka;
        }
    }
}