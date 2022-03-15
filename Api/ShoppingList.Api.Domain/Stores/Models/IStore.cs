﻿using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Updates;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

public interface IStore
{
    StoreId Id { get; }
    string Name { get; }
    bool IsDeleted { get; }
    IReadOnlyCollection<IStoreSection> Sections { get; }

    void ChangeName(string name);

    bool ContainsSection(SectionId sectionId);

    IStoreSection GetDefaultSection();

    void UpdateSections(IEnumerable<SectionUpdate> sectionUpdates);
}