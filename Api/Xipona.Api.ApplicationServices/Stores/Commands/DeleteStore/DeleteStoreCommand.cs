using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.ApplicationServices.Stores.Commands.DeleteStore;

public class DeleteStoreCommand : ICommand<bool>
{
    public DeleteStoreCommand(StoreId storeId)
    {
        StoreId = storeId;
    }

    public StoreId StoreId { get; }
}