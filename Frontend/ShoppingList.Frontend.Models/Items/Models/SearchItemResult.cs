using ProjectHermes.ShoppingList.Frontend.Models.Shared;
using System;

namespace ProjectHermes.ShoppingList.Frontend.Models.Items.Models
{
    public class SearchItemResult : ISearchResult
    {
        public SearchItemResult(Guid itemId, string name)
        {
            Id = itemId;
            Name = name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}