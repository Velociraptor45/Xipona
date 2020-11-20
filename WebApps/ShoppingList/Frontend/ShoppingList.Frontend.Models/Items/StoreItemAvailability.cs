namespace ShoppingList.Frontend.Models.Items
{
    public class StoreItemAvailability
    {
        public StoreItemAvailability(int storeId, float pricePerQuantity)
        {
            StoreId = storeId;
            PricePerQuantity = pricePerQuantity;
        }

        public int StoreId { get; set; }
        public float PricePerQuantity { get; set; }
    }
}