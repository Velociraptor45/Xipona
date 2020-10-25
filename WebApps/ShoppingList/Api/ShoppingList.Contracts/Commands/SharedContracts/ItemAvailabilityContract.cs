namespace ShoppingList.Contracts.Commands.SharedContracts.CreateItem
{
    public class ItemAvailabilityContract
    {
        public int StoreId { get; set; }
        public float Price { get; set; }
    }
}