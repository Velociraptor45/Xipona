using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Stores.Commands.DeleteStore;

public class DeleteStoreCommand : ICommand<bool>
{
    public DeleteStoreCommand(StoreId storeId)
    {
        StoreId = storeId;
    }

    public StoreId StoreId { get; }
}