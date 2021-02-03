namespace ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared
{
    public class ItemAvailabilityContract
    {
        public int StoreId { get; set; }
        public float Price { get; set; }
        public int DefaultSectionId { get; set; }
    }
}