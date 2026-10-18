using System;
using System.Collections.Generic;
using System.Text;

namespace IW5_project.Common.Models.ShoppingList
{
    public class ShoppingListListModel : IWithId
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
        public string? Description { get; init; }
        public int ItemCount { get; init; }
        public string Store { get; init; } = null!;
    }
}
