namespace ShoppingList.Frontend.Models.Items
{
    public class StoreItemAvailabilities
    {
        public StoreItemAvailabilities(int storeId, float pricePerQuantity)
        {
            StoreId = storeId;
            PricePerQuantity = pricePerQuantity;
        }

        public int StoreId { get; set; }
        public float PricePerQuantity { get; set; }
    }
}