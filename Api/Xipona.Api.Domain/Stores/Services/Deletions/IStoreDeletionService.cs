using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.Stores.Services.Deletions;

public interface IStoreDeletionService
{
    Task DeleteAsync(StoreId storeId);
}