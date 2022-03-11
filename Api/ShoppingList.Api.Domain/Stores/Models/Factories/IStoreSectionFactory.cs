using ProjectHermes.ShoppingList.Api.Domain.Stores.Commands.CreateStore;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models.Factories;

public interface IStoreSectionFactory
{
    IStoreSection Create(SectionId id, string name, int sortingIndex, bool isDefaultSection);
    IStoreSection CreateNew(SectionCreationInfo creationInfo);
}