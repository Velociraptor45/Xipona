using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.Domain.Stores.Services.Deletions;

public interface IStoreDeletionService
{
    Task DeleteAsync(StoreId storeId);
}