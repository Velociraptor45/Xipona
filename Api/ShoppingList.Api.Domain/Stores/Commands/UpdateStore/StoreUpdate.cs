using System.Collections.Generic;
using System.Linq;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Commands.UpdateStore;

public class StoreUpdate
{
    public StoreUpdate(StoreId id, string name, IEnumerable<IStoreSection> sections)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new System.ArgumentException($"'{nameof(name)}' cannot be null or whitespace", nameof(name));
        }

        Id = id;
        Name = name;
        Sections = sections.ToList().AsReadOnly();
    }

    public StoreId Id { get; }
    public string Name { get; }
    public IReadOnlyCollection<IStoreSection> Sections { get; }
}