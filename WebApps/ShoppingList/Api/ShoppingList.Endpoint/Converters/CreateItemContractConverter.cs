using ShoppingList.Contracts.CreateItem;
using ShoppingList.Domain.Models;
using System.Collections.Generic;

namespace ShoppingList.Endpoint.Converters
{
    public static class CreateItemContractConverter
    {
        public static IEnumerable<StoreItem> ToDomain(this CreateItemContract contract)
        {
            foreach (var store in contract.ItemInStores)
            {
                yield return new StoreItem(new StoreItemId(0),
                    contract.Name,
                    false,
                    contract.Comment,
                    contract.IsTemporary,
                    store.Price,
                    (QuantityType)contract.QuantityType,
                    contract.QuantityInPacket,
                    (QuantityTypeInPacket)contract.QuantityTypeInPacket,
                    new ItemCategoryId(contract.ItemCategoryId),
                    new ManufacturerId(contract.ManufacturerId),
                    new StoreId(store.StoreId));
            }
        }
    }
}