using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemQueries;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Queries.ItemById;

public class ItemByIdQuery : IQuery<StoreItemReadModel>
{
    public ItemByIdQuery(ItemId itemId)
    {
        ItemId = itemId;
    }

    public ItemId ItemId { get; }
}