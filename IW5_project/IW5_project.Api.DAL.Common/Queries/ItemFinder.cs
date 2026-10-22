/*using IW5_project.API.Common.Models;

namespace IW5_project.IW5_project.Api.DAL.Common.Queries
{
    public class ItemFinder
    {
        public List<ItemDTO> FindItem(string target, List<ItemDTO> list)
        {
            if (string.IsNullOrEmpty(target))
                return new List<ItemDTO>();

            return list.Where(item => item.Name.ToLower().Contains(target.ToLower())).ToList();
        }

        public List<int> FindItemIds(string target, List<ItemDTO> list)
        {
            return FindItem(target, list).Select(item => item.Id).ToList();
        }
    }
}
*/