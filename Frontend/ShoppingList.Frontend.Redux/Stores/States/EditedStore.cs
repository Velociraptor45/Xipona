using ProjectHermes.ShoppingList.Frontend.Redux.Shared.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Stores.States;

public record EditedStore(Guid Id, string Name, SortedSet<EditedSection> Sections)
    : ISortable<EditedSection>
{
    public int MinSortingIndex => Sections.Any() ? Sections.Min(s => s.SortingIndex) : 0;
    public int MaxSortingIndex => Sections.Any() ? Sections.Max(s => s.SortingIndex) : 0;
}