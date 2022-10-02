using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models.Factories;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Models;

public static class SectionsMother
{
    public static Sections Valid(int sectionCount, ISectionFactory factory)
    {
        var defaultSection = SectionMother.Default().WithSortingIndex(0).CreateMany(1);

        if (sectionCount == 0)
            return new Sections(defaultSection, factory);

        var allSections = Enumerable
            .Range(1, sectionCount)
            .Select(index => SectionMother.NotDefault().WithSortingIndex(index).Create())
            .Concat(defaultSection)
            .ToList();

        return new Sections(allSections, factory);
    }
}