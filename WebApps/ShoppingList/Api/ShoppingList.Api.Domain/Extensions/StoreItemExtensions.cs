using ShoppingList.Api.Domain.Exceptions;
using ShoppingList.Api.Domain.Models;
using ShoppingList.Api.Domain.Queries.ItemFilterResults;
using ShoppingList.Api.Domain.Queries.ItemSearch;
using ShoppingList.Api.Domain.Queries.SharedModels;
using System.Linq;

namespace ShoppingList.Api.Domain.Extensions
{
    public static class StoreItemExtensions
    {
        public static ShoppingListItem ToShoppingListItemDomain(this StoreItem storeItem, float price,
            bool isInBasket, float quantity)
        {
            return new ShoppingListItem(storeItem.Id.ToShoppingListItemId(),
                storeItem.Name,
                storeItem.IsDeleted,
                storeItem.Comment,
                storeItem.IsTemporary,
                price,
                storeItem.QuantityType,
                storeItem.QuantityInPacket,
                storeItem.QuantityTypeInPacket,
                storeItem.ItemCategory,
                storeItem.Manufacturer,
                isInBasket,
                quantity);
        }

        public static ItemSearchReadModel ToItemSearchReadModel(this StoreItem storeItem, StoreId storeId)
        {
            StoreItemAvailability storeAvailability = storeItem.Availabilities
                .FirstOrDefault(av => av.StoreId == storeId);

            if (storeAvailability == null)
                throw new ItemAtStoreNotAvailableException(storeItem.Id, storeId);

            return new ItemSearchReadModel(
                storeItem.Id.Actual,
                storeItem.Name,
                storeAvailability.Price,
                storeItem.Manufacturer,
                storeItem.ItemCategory);
        }

        public static ItemFilterResultReadModel ToItemFilterResultReadModel(this StoreItem model)
        {
            return new ItemFilterResultReadModel(model.Id.Actual, model.Name);
        }

        public static StoreItemReadModel ToReadModel(this StoreItem model)
        {
            return new StoreItemReadModel(
                model.Id.Actual,
                model.Name,
                model.IsDeleted,
                model.Comment,
                model.IsTemporary,
                model.QuantityType.ToReadModel(),
                model.QuantityInPacket,
                model.QuantityTypeInPacket.ToReadModel(),
                model.ItemCategory?.ToReadModel(),
                model.Manufacturer?.ToReadModel(),
                model.Availabilities.Select(av => av.ToReadModel()));
        }
    }
}