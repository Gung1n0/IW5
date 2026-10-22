using IW5_project.Common.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace IW5_project.Common.Models.Common
{
    public class ItemQuery : PagedQueryBase
    {
        public ItemQuery(
            Guid? PlaceId,
            ItemCategories Category = ItemCategories.Drogerie,
            ItemSortBy Sort = ItemSortBy.Name
            )
        { }
    }
}
