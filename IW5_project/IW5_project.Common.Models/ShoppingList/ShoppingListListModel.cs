using System;
using System.Collections.Generic;
using System.Text;

namespace IW5_project.Common.Models.ShoppingList
{
    public class ShoppingListListModel : IWithId
    {
        public Guid Id { get; init; }
        public string name { get; init; } = null!;
        public string? description { get; init; }
        public int quantity { get; init; }

    }
}
