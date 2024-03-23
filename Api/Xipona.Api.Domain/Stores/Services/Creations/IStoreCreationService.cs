using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.Stores.Services.Creations;

public interface IStoreCreationService
{
    Task<IStore> CreateAsync(StoreCreation creation);
}