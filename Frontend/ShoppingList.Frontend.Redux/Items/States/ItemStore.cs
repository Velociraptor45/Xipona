namespace ShoppingList.Frontend.Redux.Items.States;

public record ItemStore(Guid Id, string Name, IReadOnlyCollection<ItemStoreSection> Sections)
{
    public Guid DefaultSectionId => Sections.First(s => s.IsDefaultSection).Id;
}