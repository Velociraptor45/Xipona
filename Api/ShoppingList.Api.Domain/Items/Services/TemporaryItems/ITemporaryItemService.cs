namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.TemporaryItems;

public interface ITemporaryItemService
{
    Task MakePermanentAsync(PermanentItem permanentItem);
}