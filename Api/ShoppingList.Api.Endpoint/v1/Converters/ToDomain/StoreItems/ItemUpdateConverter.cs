using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.UpdateItem;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.UpdateItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.StoreItems
{
    public class ItemUpdateConverter : IToDomainConverter<UpdateItemContract, ItemUpdate>
    {
        private readonly IToDomainConverter<ItemAvailabilityContract, IStoreItemAvailability> storeItemAvailabilityConverter;

        public ItemUpdateConverter(
            IToDomainConverter<ItemAvailabilityContract, IStoreItemAvailability> storeItemAvailabilityConverter)
        {
            this.storeItemAvailabilityConverter = storeItemAvailabilityConverter;
        }

        public ItemUpdate ToDomain(UpdateItemContract source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return new ItemUpdate(
                new ItemId(source.OldId),
                source.Name,
                source.Comment,
                source.QuantityType.ToEnum<QuantityType>(),
                source.QuantityInPacket,
                source.QuantityTypeInPacket.ToEnum<QuantityTypeInPacket>(),
                new ItemCategoryId(source.ItemCategoryId),
                source.ManufacturerId.HasValue ?
                    new ManufacturerId(source.ManufacturerId.Value) :
                    null,
                storeItemAvailabilityConverter.ToDomain(source.Availabilities));
        }
    }
}