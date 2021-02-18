using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using System.Collections.Generic;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures
{
    public class ShoppingListSectionDefinition
    {
        public ShoppingListSectionId Id { get; set; }
        public string Name { get; set; }
        public int? SortingIndex { get; set; }
        public bool? IsDefaultSection { get; set; }
        public IEnumerable<IShoppingListItem> Items { get; set; }
    }
}