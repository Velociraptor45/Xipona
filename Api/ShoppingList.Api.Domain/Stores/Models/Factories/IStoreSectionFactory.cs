using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Creations;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models.Factories;

public interface IStoreSectionFactory
{
    IStoreSection Create(SectionId id, SectionName name, int sortingIndex, bool isDefaultSection);

    IStoreSection CreateNew(SectionName name, int sortingIndex, bool isDefaultSection);

    IStoreSection CreateNew(SectionCreation creation);
}