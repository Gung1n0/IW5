using System.Collections.Generic;
using System.Linq;
namespace DAL
{
    public class ItemFinder
    {
        public List<PolozkaDTO> FindItem(string target, List<PolozkaDTO> list)
        {
            if (string.IsNullOrEmpty(target))
                return [];

            return list.Where(item => item.Name.ToLower().Contains(target.ToLower())).ToList();
        }

        public List<int> FindItemIds(string target, List<PolozkaDTO> list)
        {
            return FindItem(target, list).Select(item => item.Id).ToList();
        }
    }
}