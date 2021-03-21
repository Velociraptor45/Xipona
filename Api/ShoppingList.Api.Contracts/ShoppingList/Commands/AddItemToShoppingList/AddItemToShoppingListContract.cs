using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.Shared;

namespace ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.AddItemToShoppingList
{
    public class AddItemToShoppingListContract
    {
        public int ShoppingListId { get; set; }
        public ItemIdContract ItemId { get; set; }
        public int? SectionId { get; set; }
        public float Quantity { get; set; }
    }
}