using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateItem;
using System;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.StoreItems
{
    public class ItemCreationConverter : IToDomainConverter<CreateItemContract, ItemCreation>
    {
        private readonly IToDomainConverter<ItemAvailabilityContract, ShortAvailability> shortAvailabilityContract;

        public ItemCreationConverter(
            IToDomainConverter<ItemAvailabilityContract, ShortAvailability> shortAvailabilityContract)
        {
            this.shortAvailabilityContract = shortAvailabilityContract;
        }

        public ItemCreation ToDomain(CreateItemContract source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return new ItemCreation(
                source.Name,
                source.Comment,
                source.QuantityType.ToEnum<QuantityType>(),
                source.QuantityInPacket,
                source.QuantityTypeInPacket.ToEnum<QuantityTypeInPacket>(),
                new ItemCategoryId(source.ItemCategoryId),
                source.ManufacturerId.HasValue ?
                    new ManufacturerId(source.ManufacturerId.Value) :
                    null,
                shortAvailabilityContract.ToDomain(source.Availabilities));
        }
    }
}