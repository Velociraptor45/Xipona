using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.StoreCreations;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models.Factories;

public class StoreSectionFactory : IStoreSectionFactory
{
    public IStoreSection Create(SectionId id, string name, int sortingIndex, bool isDefaultSection)
    {
        return new StoreSection(id, name, sortingIndex, isDefaultSection);
    }

    public IStoreSection CreateNew(string name, int sortingIndex, bool isDefaultSection)
    {
        return Create(SectionId.New, name, sortingIndex, isDefaultSection);
    }

    public IStoreSection CreateNew(SectionCreation creation)
    {
        return Create(SectionId.New, creation.Name, creation.SortingIndex,
            creation.IsDefaultSection);
    }
}