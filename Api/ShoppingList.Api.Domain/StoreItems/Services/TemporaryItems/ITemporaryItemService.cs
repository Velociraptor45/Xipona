namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.TemporaryItems;

public interface ITemporaryItemService
{
    Task MakePermanentAsync(PermanentItem permanentItem);
}