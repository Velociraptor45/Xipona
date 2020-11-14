using ShoppingList.Api.Domain.Models;
using ShoppingList.Api.Domain.Queries.SharedModels;

namespace ShoppingList.Api.Domain.Queries.ItemById
{
    public class ItemByIdQuery : IQuery<StoreItemReadModel>
    {
        public ItemByIdQuery(StoreItemId itemId)
        {
            ItemId = itemId ?? throw new System.ArgumentNullException(nameof(itemId));
        }

        public StoreItemId ItemId { get; }
    }
}