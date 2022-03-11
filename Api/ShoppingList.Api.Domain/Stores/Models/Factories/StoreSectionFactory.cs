using ProjectHermes.ShoppingList.Api.Domain.Stores.Commands.CreateStore;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models.Factories;

public class StoreSectionFactory : IStoreSectionFactory
{
    public IStoreSection Create(SectionId id, string name, int sortingIndex, bool isDefaultSection)
    {
        return new StoreSection(id, name, sortingIndex, isDefaultSection);
    }

    public IStoreSection CreateNew(SectionCreationInfo creationInfo)
    {
        return Create(new SectionId(Guid.NewGuid()), creationInfo.Name, creationInfo.SortingIndex,
            creationInfo.IsDefaultSection);
    }
}