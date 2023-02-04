using ProjectHermes.ShoppingList.Frontend.Redux.Items.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.Items
{
    public class MakeTemporaryItemPermanentRequest
    {
        public MakeTemporaryItemPermanentRequest(Guid itemId, string name, string comment, int quantityType,
            float? quantityInPacket, int? quantityTypeInPacket, Guid itemCategoryId, Guid? manufacturerId,
            IReadOnlyCollection<EditedItemAvailability> availabilities)
        {
            ItemId = itemId;
            Name = name;
            Comment = comment;
            QuantityType = quantityType;
            QuantityInPacket = quantityInPacket;
            QuantityTypeInPacket = quantityTypeInPacket;
            ItemCategoryId = itemCategoryId;
            ManufacturerId = manufacturerId;
            Availabilities = availabilities;
        }

        public Guid ItemId { get; }
        public string Name { get; }
        public string Comment { get; }
        public int QuantityType { get; }
        public float? QuantityInPacket { get; }
        public int? QuantityTypeInPacket { get; }
        public Guid ItemCategoryId { get; }
        public Guid? ManufacturerId { get; }
        public IReadOnlyCollection<EditedItemAvailability> Availabilities { get; }
    }
}