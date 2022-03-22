using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Queries.ItemById;

public class ItemByIdQuery : IQuery<StoreItemReadModel>
{
    public ItemByIdQuery(ItemId itemId)
    {
        ItemId = itemId;
    }

    public ItemId ItemId { get; }
}