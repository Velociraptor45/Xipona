using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Creations;

public class StoreCreation
{
    public StoreCreation(StoreName name, IEnumerable<SectionCreation> sections)
    {
        Name = name;
        Sections = sections.ToArray();
    }

    public StoreName Name { get; }
    public IReadOnlyCollection<SectionCreation> Sections { get; }
}