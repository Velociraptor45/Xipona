using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.ItemSearch;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemSearch;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;
using System;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.StoreItem
{
    public class ItemSearchContractConverter :
        IToContractConverter<ItemSearchReadModel, ItemSearchContract>
    {
        private readonly IToContractConverter<StoreItemSectionReadModel, StoreItemSectionContract> storeItemSectionContractConverter;

        public ItemSearchContractConverter(
            IToContractConverter<StoreItemSectionReadModel, StoreItemSectionContract> storeItemSectionContractConverter)
        {
            this.storeItemSectionContractConverter = storeItemSectionContractConverter;
        }

        public ItemSearchContract ToContract(ItemSearchReadModel source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return new ItemSearchContract(
                source.Id.Value,
                source.Name,
                source.DefaultQuantity,
                source.Price,
                source.ItemCategory.Name,
                source.Manufacturer?.Name ?? "",
                storeItemSectionContractConverter.ToContract(source.DefaultSection));
        }
    }
}