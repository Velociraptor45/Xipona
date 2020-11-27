using System.Linq;

namespace ShoppingList.Api.Infrastructure.Extensions.Entities
{
    public static class StoreItemExtensions
    {
        public static Domain.Models.StoreItem ToStoreItemDomain(this Infrastructure.Entities.Item entity)
        {
            return new Domain.Models.StoreItem(
                new Domain.Models.StoreItemId(entity.Id),
                entity.Name,
                entity.Deleted,
                entity.Comment,
                entity.IsTemporary,
                (Domain.Models.QuantityType)entity.QuantityType,
                entity.QuantityInPacket,
                (Domain.Models.QuantityTypeInPacket)entity.QuantityTypeInPacket,
                entity.ItemCategory?.ToDomain(),
                entity.Manufacturer?.ToDomain(),
                entity.AvailableAt.Select(map => new Domain.Models.StoreItemAvailability(
                    new Domain.Models.StoreId(map.StoreId), map.Price)));
        }
    }
}