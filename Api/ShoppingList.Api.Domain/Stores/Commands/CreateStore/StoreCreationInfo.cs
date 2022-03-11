namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Commands.CreateStore;

public class StoreCreationInfo
{
    public StoreCreationInfo(string name, IEnumerable<SectionCreationInfo> sections)
    {
        Name = name;
        Sections = sections.ToList().AsReadOnly();
    }

    public string Name { get; }
    public IReadOnlyCollection<SectionCreationInfo> Sections { get; }
}