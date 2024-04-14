﻿using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.Items.Models;

public static class ItemTypeMother
{
    public static ItemTypeBuilder Initial()
    {
        return new ItemTypeBuilder()
            .WithIsDeleted(false)
            .WithoutPredecessorId();
    }

    public static ItemTypeBuilder WithPredecessor()
    {
        return new ItemTypeBuilder()
            .WithIsDeleted(false);
    }

    public static ItemTypeBuilder InitialAvailableAt(ItemAvailability availability)
    {
        return new ItemTypeBuilder()
            .WithAvailabilities(availability.ToMonoList())
            .WithIsDeleted(false)
            .WithoutPredecessorId();
    }

    public static ItemTypeBuilder InitialAvailableAt(StoreId storeId)
    {
        var av = ItemAvailabilityMother.ForStore(storeId).Create();
        return InitialAvailableAt(av);
    }

    public static ItemTypeBuilder InitialAvailableAt(SectionId defaultSectionId)
    {
        var av = ItemAvailabilityMother.ForDefaultSection(defaultSectionId).Create();
        return InitialAvailableAt(av);
    }
}