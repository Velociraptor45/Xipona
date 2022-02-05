using System.Collections.Generic;
using System.Linq;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Commands.CreateStore;

public class StoreCreationInfo
{
    public StoreCreationInfo(StoreId id, string name, IEnumerable<IStoreSection> sections)
    {
        Id = id;
        Name = name;
        Sections = sections.ToList().AsReadOnly();
    }

    public StoreId Id { get; }
    public string Name { get; }
    public IReadOnlyCollection<IStoreSection> Sections { get; }
}