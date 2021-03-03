using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.AllQuantityTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.AllQuantityTypes;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.AllQuantityTypesInPacket;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;
using System;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.StoreItem
{
    public class StoreItemContractConverter :
        IToContractConverter<StoreItemReadModel, StoreItemContract>
    {
        private readonly IToContractConverter<StoreItemAvailabilityReadModel, StoreItemAvailabilityContract> storeItemAvailabilityContractConverter;
        private readonly IToContractConverter<ItemCategoryReadModel, ItemCategoryContract> itemCategoryContractConverter;
        private readonly IToContractConverter<ManufacturerReadModel, ManufacturerContract> manufacturerContractConverter;
        private readonly IToContractConverter<QuantityTypeReadModel, QuantityTypeContract> quantityTypeContractConverter;
        private readonly IToContractConverter<QuantityTypeInPacketReadModel, QuantityTypeInPacketContract> quantityTypeInPacketContractConverter;

        public StoreItemContractConverter(
            IToContractConverter<StoreItemAvailabilityReadModel, StoreItemAvailabilityContract> storeItemAvailabilityContractConverter,
            IToContractConverter<ItemCategoryReadModel, ItemCategoryContract> itemCategoryContractConverter,
            IToContractConverter<ManufacturerReadModel, ManufacturerContract> manufacturerContractConverter,
            IToContractConverter<QuantityTypeReadModel, QuantityTypeContract> quantityTypeContractConverter,
            IToContractConverter<QuantityTypeInPacketReadModel, QuantityTypeInPacketContract> quantityTypeInPacketContractConverter)
        {
            this.storeItemAvailabilityContractConverter = storeItemAvailabilityContractConverter;
            this.itemCategoryContractConverter = itemCategoryContractConverter;
            this.manufacturerContractConverter = manufacturerContractConverter;
            this.quantityTypeContractConverter = quantityTypeContractConverter;
            this.quantityTypeInPacketContractConverter = quantityTypeInPacketContractConverter;
        }

        public StoreItemContract ToContract(StoreItemReadModel source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            ItemCategoryContract itemCategoryContract = null;
            if (source.ItemCategory != null)
                itemCategoryContract = itemCategoryContractConverter.ToContract(source.ItemCategory);

            ManufacturerContract manufacturerContract = null;
            if (source.Manufacturer != null)
                manufacturerContract = manufacturerContractConverter.ToContract(source.Manufacturer);

            return new StoreItemContract(
                source.Id.Value,
                source.Name,
                source.IsDeleted,
                source.Comment,
                source.IsTemporary,
                quantityTypeContractConverter.ToContract(source.QuantityType),
                source.QuantityInPacket,
                quantityTypeInPacketContractConverter.ToContract(source.QuantityTypeInPacket),
                itemCategoryContract,
                manufacturerContract,
                storeItemAvailabilityContractConverter.ToContract(source.Availabilities));
        }
    }
}