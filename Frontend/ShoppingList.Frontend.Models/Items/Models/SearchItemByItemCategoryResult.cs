using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Models.Items.Models;

public class SearchItemByItemCategoryResult
{
    public SearchItemByItemCategoryResult(Guid itemId, Guid? itemTypeId, string name,
        IEnumerable<SearchItemByItemCategoryAvailability> availabilities)
    {
        ItemId = itemId;
        ItemTypeId = itemTypeId;
        Name = name;
        Availabilities = availabilities.ToList();
    }

    public Guid ItemId { get; }
    public Guid? ItemTypeId { get; }
    public string Name { get; }
    public IReadOnlyCollection<SearchItemByItemCategoryAvailability> Availabilities { get; }

    public string SelectIdentifier
    {
        get => $"{ItemId}{ItemTypeId?.ToString() ?? string.Empty}";
        set { _ = value; }
    }
}