using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.Stores.Services.Modifications;

namespace Xipona.Api.ApplicationServices.Stores.Commands.ModifyStore;

public class ModifyStoreCommand : ICommand<bool>
{
    public ModifyStoreCommand(StoreModification storeUpdate)
    {
        StoreUpdate = storeUpdate;
    }

    public StoreModification StoreUpdate { get; }
}