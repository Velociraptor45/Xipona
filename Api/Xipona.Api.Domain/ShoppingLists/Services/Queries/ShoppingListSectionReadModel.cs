﻿using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Queries;

public class ShoppingListSectionReadModel
{
    private readonly IEnumerable<ShoppingListItemReadModel> _itemReadModels;

    public ShoppingListSectionReadModel(SectionId id, SectionName name, int sortingIndex,
        bool isDefaultSection, IEnumerable<ShoppingListItemReadModel> itemReadModels)
    {
        Id = id;
        Name = name;
        SortingIndex = sortingIndex;
        IsDefaultSection = isDefaultSection;
        _itemReadModels = itemReadModels;
    }

    public IReadOnlyCollection<ShoppingListItemReadModel> ItemReadModels => _itemReadModels.ToList().AsReadOnly();

    public SectionId Id { get; }
    public SectionName Name { get; }
    public int SortingIndex { get; }
    public bool IsDefaultSection { get; }
}