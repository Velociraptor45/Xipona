using AutoFixture.Kernel;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common.AutoFixture.Selectors;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.Items.Models;

public class ItemBuilder : DomainTestBuilderBase<Item>
{
    public ItemBuilder()
    {
        Customize(new ItemQuantityCustomization());
        Customize(new PriceCustomization());
        Customize(new QuantityCustomization());
    }

    public ItemBuilder AsItem()
    {
        Customize<Item>(c => c.FromFactory(new MethodInvoker(new ItemConstructorQuery())));
        return this;
    }

    public ItemBuilder WithId(ItemId id)
    {
        FillConstructorWith("id", id);
        return this;
    }

    public ItemBuilder WithIsDeleted(bool isDeleted)
    {
        FillConstructorWith("isDeleted", isDeleted);
        return this;
    }

    public ItemBuilder WithIsTemporary(bool isTemporary)
    {
        FillConstructorWith("isTemporary", isTemporary);
        return this;
    }

    public ItemBuilder WithItemQuantity(ItemQuantity itemQuantity)
    {
        FillConstructorWith(nameof(itemQuantity), itemQuantity);
        return this;
    }

    public ItemBuilder WithItemCategoryId(ItemCategoryId? itemCategoryId)
    {
        FillConstructorWith("itemCategoryId", itemCategoryId);
        return this;
    }

    public ItemBuilder WithoutItemCategoryId()
    {
        return WithItemCategoryId(null);
    }

    public ItemBuilder WithManufacturerId(ManufacturerId? manufacturerId)
    {
        FillConstructorWith("manufacturerId", manufacturerId);
        return this;
    }

    public ItemBuilder WithoutManufacturerId()
    {
        return WithManufacturerId(null);
    }

    public ItemBuilder WithAvailabilities(IEnumerable<ItemAvailability> availabilities)
    {
        FillConstructorWith("availabilities", availabilities);
        return this;
    }

    public ItemBuilder WithAvailability(ItemAvailability availability)
    {
        return WithAvailabilities([availability]);
    }

    public ItemBuilder WithTemporaryId(TemporaryItemId? temporaryId)
    {
        FillConstructorWith("temporaryId", temporaryId);
        return this;
    }

    public ItemBuilder WithoutTemporaryId()
    {
        return WithTemporaryId(null);
    }

    public ItemBuilder WithTypes(ItemTypes itemTypes)
    {
        FillConstructorWith("itemTypes", itemTypes);
        return this;
    }

    public ItemBuilder WithCreatedAt(DateTimeOffset createdAt)
    {
        FillConstructorWith(nameof(createdAt), createdAt);
        return this;
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
}