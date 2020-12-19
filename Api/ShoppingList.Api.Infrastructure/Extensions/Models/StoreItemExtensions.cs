using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Extensions.Models
{
    public static class StoreItemExtensions
    {
        public static Infrastructure.Entities.Item ToEntity(this IStoreItem model)
        {
            return new Infrastructure.Entities.Item()
            {
                Id = model.Id.Actual?.Value ?? 0,
                Name = model.Name,
                Deleted = model.IsDeleted,
                Comment = model.Comment,
                IsTemporary = model.IsTemporary,
                QuantityType = (int)model.QuantityType,
                QuantityInPacket = model.QuantityInPacket,
                QuantityTypeInPacket = (int)model.QuantityTypeInPacket,
                ItemCategoryId = model.ItemCategory?.Id.Value,
                ManufacturerId = model.Manufacturer?.Id.Value,
                CreatedFrom = model.Id.Offline?.Value,
                Manufacturer = model.Manufacturer?.ToEntity(),
                ItemCategory = model.ItemCategory?.ToEntity(),
                AvailableAt = model.Availabilities.Select(av => av.ToEntity(model.Id)).ToList(),
                PredecessorId = model.Predecessor?.Id.Actual.Value
            };
        }
    }
}