using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Creations;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models.Factories;

public class SectionFactory : ISectionFactory
{
    public ISection Create(SectionId id, SectionName name, int sortingIndex, bool isDefaultSection)
    {
        return new Section(id, name, sortingIndex, isDefaultSection);
    }

    public ISection CreateNew(SectionName name, int sortingIndex, bool isDefaultSection)
    {
        return Create(SectionId.New, name, sortingIndex, isDefaultSection);
    }

    public ISection CreateNew(SectionCreation creation)
    {
        return Create(SectionId.New, creation.Name, creation.SortingIndex,
            creation.IsDefaultSection);
    }
}