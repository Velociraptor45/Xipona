using ProjectHermes.ShoppingList.Api.Core.Attributes;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemFilterResults;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemSearch;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Extensions
{
    public static class StoreItemExtensions
    {
        public static ShoppingListItem ToShoppingListItemDomain(this IStoreItem storeItem, float price,
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

        public static ItemSearchReadModel ToItemSearchReadModel(this IStoreItem storeItem, StoreId storeId)
        {
            IStoreItemAvailability storeAvailability = storeItem.Availabilities
                .FirstOrDefault(av => av.StoreId == storeId);

            if (storeAvailability == null)
                throw new ItemAtStoreNotAvailableException(storeItem.Id, storeId);

            return new ItemSearchReadModel(
                storeItem.Id.Actual,
                storeItem.Name,
                storeItem.QuantityType.GetAttribute<DefaultQuantityAttribute>().DefaultQuantity,
                storeAvailability.Price,
                storeItem.Manufacturer.ToReadModel(),
                storeItem.ItemCategory.ToReadModel());
        }

        public static ItemFilterResultReadModel ToItemFilterResultReadModel(this IStoreItem model)
        {
            return new ItemFilterResultReadModel(model.Id.Actual, model.Name);
        }

        public static StoreItemReadModel ToReadModel(this IStoreItem model)
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