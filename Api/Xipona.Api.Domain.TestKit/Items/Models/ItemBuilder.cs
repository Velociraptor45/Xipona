using AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Manufacturers.Models;
using Xipona.Api.Domain.TestKit.Common;
using Xipona.Api.Domain.TestKit.Common.AutoFixture.Selectors;

namespace Xipona.Api.Domain.TestKit.Items.Models;
public class ItemBuilder : DomainTestBuilderBase<Item>
{
    public ItemBuilder()
    {
        Customize(new ItemQuantityCustomization());
        Customize(new PriceCustomization());
        Customize(new QuantityCustomization());
    }

    // tcg keep
    public ItemBuilder AsItem()
    {
        Customize<Item>(c => c.FromFactory(new MethodInvoker(new ItemConstructorQuery())));
        return this;
    }

    public ItemBuilder WithId(ItemId id)
    {
        FillConstructorWith(nameof(id), id);
        return this;
    }

    public ItemBuilder WithName(ItemName name)
    {
        FillConstructorWith(nameof(name), name);
        return this;
    }

    public ItemBuilder WithIsDeleted(bool isDeleted)
    {
        FillConstructorWith(nameof(isDeleted), isDeleted);
        return this;
    }

    public ItemBuilder WithComment(Comment comment)
    {
        FillConstructorWith(nameof(comment), comment);
        return this;
    }

    public ItemBuilder WithIsTemporary(bool isTemporary)
    {
        FillConstructorWith(nameof(isTemporary), isTemporary);
        return this;
    }

    public ItemBuilder WithItemQuantity(ItemQuantity itemQuantity)
    {
        FillConstructorWith(nameof(itemQuantity), itemQuantity);
        return this;
    }

    public ItemBuilder WithItemCategoryId(ItemCategoryId? itemCategoryId)
    {
        FillConstructorWith(nameof(itemCategoryId), itemCategoryId);
        return this;
    }

    public ItemBuilder WithoutItemCategoryId()
    {
        return WithItemCategoryId(null);
    }

    public ItemBuilder WithManufacturerId(ManufacturerId? manufacturerId)
    {
        FillConstructorWith(nameof(manufacturerId), manufacturerId);
        return this;
    }

    public ItemBuilder WithoutManufacturerId()
    {
        return WithManufacturerId(null);
    }

    public ItemBuilder WithAvailabilities(IEnumerable<ItemAvailability> availabilities)
    {
        FillConstructorWith(nameof(availabilities), availabilities);
        return this;
    }

    public ItemBuilder WithAvailability(ItemAvailability availability)
    {
        return WithAvailabilities([availability]);
    }

    public ItemBuilder WithEmptyAvailabilities()
    {
        return WithAvailabilities(Enumerable.Empty<ItemAvailability>());
    }

    public ItemBuilder WithTemporaryId(TemporaryItemId? temporaryId)
    {
        FillConstructorWith(nameof(temporaryId), temporaryId);
        return this;
    }

    public ItemBuilder WithoutTemporaryId()
    {
        return WithTemporaryId(null);
    }

    public ItemBuilder WithUpdatedOn(DateTimeOffset? updatedOn)
    {
        FillConstructorWith(nameof(updatedOn), updatedOn);
        return this;
    }

    public ItemBuilder WithoutUpdatedOn()
    {
        return WithUpdatedOn(null);
    }

    public ItemBuilder WithPredecessorId(ItemId? predecessorId)
    {
        FillConstructorWith(nameof(predecessorId), predecessorId);
        return this;
    }

    public ItemBuilder WithoutPredecessorId()
    {
        return WithPredecessorId(null);
    }

    public ItemBuilder WithCreatedAt(DateTimeOffset createdAt)
    {
        FillConstructorWith(nameof(createdAt), createdAt);
        return this;
    }

    public ItemBuilder WithIsFavorite(bool isFavorite)
    {
        FillConstructorWith(nameof(isFavorite), isFavorite);
        return this;
    }

    public ItemBuilder WithItemTypes(ItemTypes itemTypes)
    {
        FillConstructorWith(nameof(itemTypes), itemTypes);
        return this;
    }
}