using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.StoreCreations;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models.Factories;

public interface IStoreSectionFactory
{
    IStoreSection Create(SectionId id, string name, int sortingIndex, bool isDefaultSection);

    IStoreSection CreateNew(SectionCreation creation);

    IStoreSection CreateNew(string name, int sortingIndex, bool isDefaultSection);
}