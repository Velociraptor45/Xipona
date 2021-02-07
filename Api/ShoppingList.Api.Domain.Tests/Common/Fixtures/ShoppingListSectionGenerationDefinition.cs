using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Fixtures
{
    public class ShoppingListSectionGenerationDefinition
    {
        public ShoppingListSectionId Id { get; set; }
        public string Name { get; set; }
        public int? SortingIndex { get; set; }
        public bool? IsDefaultSection { get; set; }
        public IEnumerable<IShoppingListItem> Items { get; set; }
    }
}