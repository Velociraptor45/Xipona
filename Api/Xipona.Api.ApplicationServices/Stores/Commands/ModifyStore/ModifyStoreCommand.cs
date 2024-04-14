using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.Stores.Services.Modifications;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Stores.Commands.ModifyStore;

public class ModifyStoreCommand : ICommand<bool>
{
    public ModifyStoreCommand(StoreModification storeUpdate)
    {
        StoreUpdate = storeUpdate;
    }

    public StoreModification StoreUpdate { get; }
}