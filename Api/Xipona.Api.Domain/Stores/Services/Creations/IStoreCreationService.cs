using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.Domain.Stores.Services.Creations;

public interface IStoreCreationService
{
    Task<IStore> CreateAsync(StoreCreation creation);
}