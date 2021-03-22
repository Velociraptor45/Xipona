using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemById
{
    public class ItemByIdQuery : IQuery<StoreItemReadModel>
    {
        public ItemByIdQuery(ItemId itemId)
        {
            ItemId = itemId ?? throw new System.ArgumentNullException(nameof(itemId));
        }

        public ItemId ItemId { get; }
    }
}