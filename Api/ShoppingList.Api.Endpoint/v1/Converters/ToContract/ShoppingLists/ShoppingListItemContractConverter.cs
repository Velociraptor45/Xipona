using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.AllQuantityTypes;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.GetActiveShoppingListByStoreId;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.AllQuantityTypes;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.AllQuantityTypesInPacket;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.SharedModels;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.ShoppingLists
{
    public class ShoppingListItemContractConverter : IToContractConverter<ShoppingListItemReadModel, ShoppingListItemContract>
    {
        private readonly IToContractConverter<ItemCategoryReadModel, ItemCategoryContract> itemCategoryContractConverter;
        private readonly IToContractConverter<ManufacturerReadModel, ManufacturerContract> manufacturerContractConverter;
        private readonly IToContractConverter<QuantityTypeReadModel, QuantityTypeContract> quantityTypeContractConverter;
        private readonly IToContractConverter<QuantityTypeInPacketReadModel, QuantityTypeInPacketContract> quantityTypeInPacketContractConverter;

        public ShoppingListItemContractConverter(
            IToContractConverter<ItemCategoryReadModel, ItemCategoryContract> itemCategoryContractConverter,
            IToContractConverter<ManufacturerReadModel, ManufacturerContract> manufacturerContractConverter,
            IToContractConverter<QuantityTypeReadModel, QuantityTypeContract> quantityTypeContractConverter,
            IToContractConverter<QuantityTypeInPacketReadModel, QuantityTypeInPacketContract> quantityTypeInPacketContractConverter)
        {
            this.itemCategoryContractConverter = itemCategoryContractConverter;
            this.manufacturerContractConverter = manufacturerContractConverter;
            this.quantityTypeContractConverter = quantityTypeContractConverter;
            this.quantityTypeInPacketContractConverter = quantityTypeInPacketContractConverter;
        }

        public ShoppingListItemContract ToContract(ShoppingListItemReadModel source)
        {
            if (source is null)
                throw new System.ArgumentNullException(nameof(source));

            ItemCategoryContract itemCategoryContract = null;
            if (source.ItemCategory != null)
                itemCategoryContract = itemCategoryContractConverter.ToContract(source.ItemCategory);

            ManufacturerContract manufacturerContract = null;
            if (source.Manufacturer != null)
                manufacturerContract = manufacturerContractConverter.ToContract(source.Manufacturer);

            QuantityTypeContract quantityTypeContract = quantityTypeContractConverter.ToContract(source.QuantityType);
            QuantityTypeInPacketContract quantityTypeInPacketContract =
                quantityTypeInPacketContractConverter.ToContract(source.QuantityTypeInPacket);

            return new ShoppingListItemContract(
                source.Id.Value,
                source.Name,
                source.IsDeleted,
                source.Comment,
                source.IsTemporary,
                source.PricePerQuantity,
                quantityTypeContract,
                source.QuantityInPacket,
                quantityTypeInPacketContract,
                itemCategoryContract,
                manufacturerContract,
                source.IsInBasket,
                source.Quantity);
        }
    }
}