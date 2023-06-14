using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Stores.Commands.DeleteStore;

public class DeleteStoreCommand : ICommand<bool>
{
    public DeleteStoreCommand(StoreId storeId)
    {
        StoreId = storeId;
    }

    public StoreId StoreId { get; }
}