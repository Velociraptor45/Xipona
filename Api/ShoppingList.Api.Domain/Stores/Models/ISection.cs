using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Updates;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

public interface ISection
{
    SectionId Id { get; }
    SectionName Name { get; }
    int SortingIndex { get; }
    bool IsDefaultSection { get; }

    ISection Update(SectionUpdate update);
}