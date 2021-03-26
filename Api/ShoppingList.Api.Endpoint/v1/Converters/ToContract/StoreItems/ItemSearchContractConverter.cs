using ProjectHermes.ShoppingList.Api.Contracts.Store.Queries.AllActiveStores;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.ItemSearch;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemSearch;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Queries.AllActiveStores;
using System;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.StoreItems
{
    public class ItemSearchContractConverter :
        IToContractConverter<ItemSearchReadModel, ItemSearchContract>
    {
        private readonly IToContractConverter<StoreSectionReadModel, StoreSectionContract> storeItemSectionContractConverter;

        public ItemSearchContractConverter(
            IToContractConverter<StoreSectionReadModel, StoreSectionContract> storeItemSectionContractConverter)
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