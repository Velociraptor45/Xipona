namespace ShoppingList.Api.Infrastructure.Extensions.Models
{
    public static class StoreItemExtensions
    {
        public static Infrastructure.Entities.Item ToEntity(this Domain.Models.StoreItem model)
        {
            return new Infrastructure.Entities.Item()
            {
                Id = model.Id.Value,
                Name = model.Name,
                Deleted = model.IsDeleted,
                Comment = model.Comment,
                IsTemporary = model.IsTemporary,
                QuantityType = (int)model.QuantityType,
                QuantityInPacket = model.QuantityInPacket,
                QuantityTypeInPacket = (int)model.QuantityTypeInPacket,
                ItemCategoryId = model.ItemCategory.Id.Value,
                ManufacturerId = model.Manufacturer.Id.Value
            };
        }
    }
}