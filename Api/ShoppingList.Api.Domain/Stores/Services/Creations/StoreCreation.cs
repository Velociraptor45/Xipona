﻿namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Creations;

public class StoreCreation
{
    public StoreCreation(string name, IEnumerable<SectionCreation> sections)
    {
        Name = name;
        Sections = sections.ToList().AsReadOnly();
    }

    public string Name { get; }
    public IReadOnlyCollection<SectionCreation> Sections { get; }
}