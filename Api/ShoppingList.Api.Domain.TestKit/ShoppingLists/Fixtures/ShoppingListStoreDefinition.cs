using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures
{
    public class ShoppingListStoreDefinition
    {
        public ShoppingListStoreId Id { get; set; }
        public string Name { get; set; }
        public bool? IsDeleted { get; set; }

        public ShoppingListStoreDefinition Clone()
        {
            return new ShoppingListStoreDefinition
            {
                Id = Id,
                Name = Name,
                IsDeleted = IsDeleted
            };
        }

        public static ShoppingListStoreDefinition FromId(int id)
        {
            return FromId(new ShoppingListStoreId(id));
        }

        public static ShoppingListStoreDefinition FromId(ShoppingListStoreId id)
        {
            return new ShoppingListStoreDefinition
            {
                Id = id
            };
        }
    }
}