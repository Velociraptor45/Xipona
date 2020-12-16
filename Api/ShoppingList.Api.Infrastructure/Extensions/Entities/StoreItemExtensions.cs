using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.Models;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Extensions.Entities
{
    public static class StoreItemExtensions
    {
        public static IStoreItem ToStoreItemDomain(this Infrastructure.Entities.Item entity)
        {
            StoreItemId id;
            if (entity.CreatedFrom == null)
                id = new StoreItemId(entity.Id);
            else
                id = new StoreItemId(entity.Id, entity.CreatedFrom.Value);

            return new StoreItem(
                id,
                entity.Name,
                entity.Deleted,
                entity.Comment,
                entity.IsTemporary,
                (QuantityType)entity.QuantityType,
                entity.QuantityInPacket,
                (QuantityTypeInPacket)entity.QuantityTypeInPacket,
                entity.ItemCategory?.ToDomain(),
                entity.Manufacturer?.ToDomain(),
                entity.AvailableAt.Select(map => new StoreItemAvailability(
                    new StoreId(map.StoreId), map.Price)),
                entity.Predecessor?.ToStoreItemDomain());
        }
    }
}