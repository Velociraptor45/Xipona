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

        public ShoppingListSectionDefinition Clone()
        {
            return new ShoppingListSectionDefinition
            {
                Id = Id,
                Name = Name,
                SortingIndex = SortingIndex,
                IsDefaultSection = IsDefaultSection,
                Items = Items
            };
        }

        public static ShoppingListSectionDefinition FromId(int id)
        {
            return FromId(new ShoppingListSectionId(id));
        }

        public static ShoppingListSectionDefinition FromId(ShoppingListSectionId id)
        {
            return new ShoppingListSectionDefinition
            {
                Id = id
            };
        }
    }
}