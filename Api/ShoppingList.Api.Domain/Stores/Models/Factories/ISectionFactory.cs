using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Creations;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models.Factories;

public interface ISectionFactory
{
    ISection Create(SectionId id, SectionName name, int sortingIndex, bool isDefaultSection);

    ISection CreateNew(SectionName name, int sortingIndex, bool isDefaultSection);

    ISection CreateNew(SectionCreation creation);
}