using System;

namespace ProjectHermes.ShoppingList.Frontend.Models.Items.Models
{
    public class SearchItemResult
    {
        public SearchItemResult(Guid itemId, string name)
        {
            ItemId = itemId;
            Name = name;
        }

        public Guid ItemId { get; }
        public string Name { get; }
    }
}