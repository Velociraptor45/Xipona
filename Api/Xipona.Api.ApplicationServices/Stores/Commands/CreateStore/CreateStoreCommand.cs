using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Services.Creations;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Stores.Commands.CreateStore;

public class CreateStoreCommand : ICommand<IStore>
{
    public CreateStoreCommand(StoreCreation storeCreation)
    {
        StoreCreation = storeCreation;
    }

    public StoreCreation StoreCreation { get; }
}