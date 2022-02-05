using System;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.ItemFilterResults;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemFilterResults;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.StoreItems;

public class ItemFilterResultContractConverter :
    IToContractConverter<ItemFilterResultReadModel, ItemFilterResultContract>
{
    public ItemFilterResultContract ToContract(ItemFilterResultReadModel source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        return new ItemFilterResultContract(source.Id.Value, source.ItemName);
    }
}