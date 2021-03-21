using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.UpdateItem;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.UpdateItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.StoreItems
{
    public class ItemUpdateConverter : IToDomainConverter<UpdateItemContract, ItemUpdate>
    {
        private readonly IToDomainConverter<ItemAvailabilityContract, ShortAvailability> shortAvailabilityConverter;

        public ItemUpdateConverter(
            IToDomainConverter<ItemAvailabilityContract, ShortAvailability> shortAvailabilityConverter)
        {
            this.shortAvailabilityConverter = shortAvailabilityConverter;
        }

        public ItemUpdate ToDomain(UpdateItemContract source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return new ItemUpdate(
                new StoreItemId(source.OldId),
                source.Name,
                source.Comment,
                source.QuantityType.ToEnum<QuantityType>(),
                source.QuantityInPacket,
                source.QuantityTypeInPacket.ToEnum<QuantityTypeInPacket>(),
                new ItemCategoryId(source.ItemCategoryId),
                source.ManufacturerId.HasValue ?
                    new ManufacturerId(source.ManufacturerId.Value) :
                    null,
                shortAvailabilityConverter.ToDomain(source.Availabilities));
        }
    }
}