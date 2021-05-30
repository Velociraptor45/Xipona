using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Converters.ToEntity
{
    public class ItemConverter : IToEntityConverter<IStoreItem, Item>
    {
        public Item ToEntity(IStoreItem source)
        {
            if (source is null)
                throw new System.ArgumentNullException(nameof(source));

            return new Item()
            {
                Id = source.Id?.Value ?? 0,
                Name = source.Name,
                Deleted = source.IsDeleted,
                Comment = source.Comment,
                IsTemporary = source.IsTemporary,
                QuantityType = source.QuantityType.ToInt(),
                QuantityInPacket = source.QuantityInPacket,
                QuantityTypeInPacket = source.QuantityTypeInPacket.ToInt(),
                ItemCategoryId = source.ItemCategoryId?.Value,
                ManufacturerId = source.ManufacturerId?.Value,
                CreatedFrom = source.TemporaryId?.Value,
                AvailableAt = source.Availabilities.Select(av => ToAvailableAt(av, source)).ToList(),
                PredecessorId = source.Predecessor?.Id.Value
            };
        }

        private AvailableAt ToAvailableAt(IStoreItemAvailability availability, IStoreItem source)
        {
            return new AvailableAt()
            {
                StoreId = availability.StoreId.Value,
                Price = availability.Price,
                ItemId = source.Id?.Value ?? 0,
                DefaultSectionId = availability.DefaultSectionId.Value
            };
        }
    }
}