using ProjectHermes.Xipona.Api.Domain.Stores.Services.Creations;

namespace ProjectHermes.Xipona.Api.Domain.Stores.Models.Factories;

public interface IStoreFactory
{
    IStore Create(StoreId id, StoreName name, bool isDeleted, IEnumerable<ISection> sections, DateTimeOffset createdAt);

    IStore CreateNew(StoreCreation creationInfo);
}