using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.Stores.Services.Creations;

public class SectionCreation
{
    public SectionCreation(SectionName name, int sortingIndex, bool isDefaultSection)
    {
        Name = name;
        SortingIndex = sortingIndex;
        IsDefaultSection = isDefaultSection;
    }

    public SectionName Name { get; }
    public int SortingIndex { get; }
    public bool IsDefaultSection { get; }
}