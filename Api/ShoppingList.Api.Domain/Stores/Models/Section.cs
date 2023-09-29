using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Modifications;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

public class Section : ISection
{
    public Section(SectionId id, SectionName name, int sortingIndex, bool isDefaultSection, bool isDeleted)
    {
        Id = id;
        Name = name;
        SortingIndex = sortingIndex;
        IsDefaultSection = isDefaultSection;
        IsDeleted = isDeleted;
    }

    public SectionId Id { get; }
    public SectionName Name { get; }
    public int SortingIndex { get; }
    public bool IsDefaultSection { get; }
    public bool IsDeleted { get; }

    public ISection Modify(SectionModification modification)
    {
        if (IsDeleted)
            throw new DomainException(new CannotModifyDeletedSectionReason(Id));

        return new Section(
            Id,
            modification.Name,
            modification.SortingIndex,
            modification.IsDefaultSection,
            IsDeleted);
    }

    public ISection Delete()
    {
        return new Section(
            Id,
            Name,
            SortingIndex,
            IsDefaultSection,
            true);
    }
}