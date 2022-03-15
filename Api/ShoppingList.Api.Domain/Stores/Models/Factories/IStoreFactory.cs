using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Creations;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models.Factories;

public interface IStoreFactory
{
    IStore Create(StoreId id, string name, bool isDeleted, IEnumerable<IStoreSection> sections);

    IStore CreateNew(StoreCreation creationInfo);
}