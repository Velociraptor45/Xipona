﻿using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.Items.Services.Queries;

public class ItemSectionReadModel
{
    public ItemSectionReadModel(SectionId id, SectionName name, int sortingIndex)
    {
        Id = id;
        Name = name;
        SortingIndex = sortingIndex;
    }

    public ItemSectionReadModel(ISection section) :
        this(section.Id, section.Name, section.SortingIndex)
    {
    }

    public SectionId Id { get; }
    public SectionName Name { get; }
    public int SortingIndex { get; }
}