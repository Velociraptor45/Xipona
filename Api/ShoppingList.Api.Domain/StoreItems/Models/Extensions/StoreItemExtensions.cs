using ProjectHermes.ShoppingList.Api.Core.Attributes;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemFilterResults;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemSearch;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Extensions
{
    public static class StoreItemExtensions
    {
        public static ItemSearchReadModel ToItemSearchReadModel(this IStoreItem storeItem, ShoppingListStoreId storeId)
        {
            IStoreItemAvailability storeAvailability = storeItem.Availabilities
                .FirstOrDefault(av => av.Store.Id == storeId);
            if (storeAvailability == null)
                throw new DomainException(new ItemAtStoreNotAvailableReason(storeItem.Id, storeId));

            var defaultSection = storeItem.GetDefaultSectionForStore(storeId.AsStoreItemStoreId());

            return new ItemSearchReadModel(
                storeItem.Id.Actual,
                storeItem.Name,
                storeItem.QuantityType.GetAttribute<DefaultQuantityAttribute>().DefaultQuantity,
                storeAvailability.Price,
                storeItem.Manufacturer.ToReadModel(),
                storeItem.ItemCategory.ToReadModel(),
                defaultSection.ToReadModel());
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