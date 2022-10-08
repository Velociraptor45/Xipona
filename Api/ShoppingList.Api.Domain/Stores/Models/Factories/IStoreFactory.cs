using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Creations;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models.Factories;

public interface IStoreFactory
{
    IStore Create(StoreId id, StoreName name, bool isDeleted, IEnumerable<ISection> sections);

    IStore CreateNew(StoreCreation creationInfo);
}