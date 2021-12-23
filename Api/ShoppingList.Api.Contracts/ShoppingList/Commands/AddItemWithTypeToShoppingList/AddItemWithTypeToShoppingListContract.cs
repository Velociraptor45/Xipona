namespace ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.AddItemWithTypeToShoppingList
{
    public class AddItemWithTypeToShoppingListContract
    {
        public int ShoppingListId { get; set; }
        public int ItemId { get; set; }
        public int ItemTypeId { get; set; }
        public int? SectionId { get; set; }
        public float Quantity { get; set; }
    }
}