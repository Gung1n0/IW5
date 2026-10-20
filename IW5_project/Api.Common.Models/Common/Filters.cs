
using IW5_project.Api.DAL.Common.Entities;
using IW5_project.API.Common.Models.Enums;

namespace IW5_project.API.Common.Models.Common
{
    public class FilterItems
    {
        //Filter items by Category
        public List<ItemDTO> FilterByCategory(List<ItemDTO> Item, ItemCategories Category)
        {
            if (Item != null)
            {
                for (int i = Item.Count - 1; i >= 0; i--)
                {
                    if (Item[i].Category != Category)
                        Item.RemoveAt(i);
                }
                return Item;
            }
            List<ItemDTO> list = [];
            return list;
        }

        //Filter items by place
        /*public List<ItemDTO> FilterByPlace(List<ItemDTO> Item, Place place)
        {
            if (Item != null)
            {
                var arr = Item;
                foreach (var item in arr)
                {
                    if (item.place != place)
                        arr.Remove(item);
                }
                return arr;
            }
            return Item;
        }*/

        //Do the same but for user, wait for user structure

        //Filter items by amount
        public List<ItemDTO> FilterByAmount(List<ItemDTO> Item, float Amount, Operator op)
        {
            if (Item != null)
            {
                var arr = Item;
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
            List<ItemDTO> list = [];
            return list;
        }
    }
}
