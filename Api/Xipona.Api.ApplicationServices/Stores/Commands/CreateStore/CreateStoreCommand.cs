using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.Stores.Models;
using Xipona.Api.Domain.Stores.Services.Creations;

namespace Xipona.Api.ApplicationServices.Stores.Commands.CreateStore;

public class CreateStoreCommand : ICommand<IStore>
{
    public CreateStoreCommand(StoreCreation storeCreation)
    {
        StoreCreation = storeCreation;
    }

    public StoreCreation StoreCreation { get; }
}