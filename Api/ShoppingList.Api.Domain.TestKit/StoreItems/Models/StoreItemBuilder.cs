using AutoFixture.Kernel;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.Common;
using ShoppingList.Api.Domain.TestKit.Common.AutoFixture.Selectors;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Models;

public class StoreItemBuilder : DomainTestBuilderBase<StoreItem>
{
    public StoreItemBuilder()
    {
        Customize(new ItemQuantityCustomization());
    }

    public StoreItemBuilder AsItem()
    {
        Customize<StoreItem>(c => c.FromFactory(new MethodInvoker(new ItemConstructorQuery())));
        return this;
    }

    public StoreItemBuilder WithId(ItemId id)
    {
        FillConstructorWith("id", id);
        return this;
    }

    public StoreItemBuilder WithIsDeleted(bool isDeleted)
    {
        FillConstructorWith("isDeleted", isDeleted);
        return this;
    }

    public StoreItemBuilder WithIsTemporary(bool isTemporary)
    {
        FillConstructorWith("isTemporary", isTemporary);
        return this;
    }

    public StoreItemBuilder WithItemQuantity(ItemQuantity itemQuantity)
    {
        FillConstructorWith(nameof(itemQuantity), itemQuantity);
        return this;
    }

    public StoreItemBuilder WithItemCategoryId(ItemCategoryId? itemCategoryId)
    {
        FillConstructorWith("itemCategoryId", itemCategoryId);
        return this;
    }

    public StoreItemBuilder WithoutItemCategoryId()
    {
        return WithItemCategoryId(null);
    }

    public StoreItemBuilder WithManufacturerId(ManufacturerId? manufacturerId)
    {
        FillConstructorWith("manufacturerId", manufacturerId);
        return this;
    }

    public StoreItemBuilder WithoutManufacturerId()
    {
        return WithManufacturerId(null);
    }

    public StoreItemBuilder WithAvailabilities(IEnumerable<IStoreItemAvailability> availabilities)
    {
        FillConstructorWith("availabilities", availabilities);
        return this;
    }

    public StoreItemBuilder WithAvailability(IStoreItemAvailability availability)
    {
        return WithAvailabilities(availability.ToMonoList());
    }

    public StoreItemBuilder WithTemporaryId(TemporaryItemId? temporaryId)
    {
        FillConstructorWith("temporaryId", temporaryId);
        return this;
    }

    public StoreItemBuilder WithoutTemporaryId()
    {
        return WithTemporaryId(null);
    }

    public StoreItemBuilder WithTypes(ItemTypes itemTypes)
    {
        FillConstructorWith("itemTypes", itemTypes);
        return this;
    }

    public StoreItemBuilder WithoutTypes()
    {
        return WithTypes(null);
    }
}