using Xipona.Api.Core.Extensions;
using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.ItemCategories.Services.Shared;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Items.Services.Queries.Quantities;
using Xipona.Api.Domain.Manufacturers.Models;
using Xipona.Api.Domain.Manufacturers.Services.Shared;
using Xipona.Api.Domain.ShoppingLists.Models;
using Xipona.Api.Domain.ShoppingLists.Services.Queries;
using Xipona.Api.Domain.Stores.Models;
using Xipona.Api.Domain.TestKit.ItemCategories.Models;
using Xipona.Api.Domain.TestKit.Items.Models;
using Xipona.Api.Domain.TestKit.Manufacturers.Models;
using Xipona.Api.Domain.TestKit.ShoppingLists.Models;
using Xipona.Api.Domain.TestKit.Stores.Models;

namespace Xipona.Api.Domain.Tests.ShoppingLists.Services.Conversion.ShoppingListReadModels;

public class ConvertAsyncTestData : TheoryData<IShoppingList, IStore, IEnumerable<IItem>, IEnumerable<IItemCategory>,
    IEnumerable<IManufacturer>, ShoppingListReadModel>
{
    public ConvertAsyncTestData()
    {
        NoItemCategory();
        NoManufacturer();
        NeitherItemCategoryNorManufacturer();
        WithItemCategoryAndManufacturer();
        EmptyList();
    }

    private void NoItemCategory()
    {
        IStore store = StoreMother.Initial().Create();
        var list = GetShoppingListContainingOneItem(store.Id, store.Sections.First().Id);
        IManufacturer manufacturer = ManufacturerMother.NotDeleted().Create();

        var availability = GetAvailabilityFrom(store);

        IItem item = new ItemBuilder()
            .WithoutItemCategoryId()
            .WithManufacturerId(manufacturer.Id)
            .WithId(list.Sections.First().Items.First().Id)
            .WithAvailabilities([availability])
            .AsItem()
            .Create();
        var listReadModel = ToSimpleReadModel(list, store, item, null, manufacturer);

        Add(
            list,
            store,
            [item],
            [],
            manufacturer.ToMonoList(),
            listReadModel);
    }

    private void NoManufacturer()
    {
        IStore store = StoreMother.Initial().Create();
        var list = GetShoppingListContainingOneItem(store.Id, store.Sections.First().Id);
        IItemCategory itemCategory = ItemCategoryMother.NotDeleted().Create();

        var availability = GetAvailabilityFrom(store);
        IItem item = new ItemBuilder()
            .WithItemCategoryId(itemCategory.Id)
            .WithoutManufacturerId()
            .WithAvailabilities([availability])
            .WithId(list.Sections.First().Items.First().Id)
            .AsItem()
            .Create();

        var listReadModel = ToSimpleReadModel(list, store, item, itemCategory, null);

        Add(
            list,
            store,
            [item],
            [itemCategory],
            [],
            listReadModel);
    }

    private void NeitherItemCategoryNorManufacturer()
    {
        IStore store = StoreMother.Initial().Create();
        var list = GetShoppingListContainingOneItem(store.Id, store.Sections.First().Id);

        var availability = GetAvailabilityFrom(store);
        IItem item = ItemMother.InitialTemporary()
            .WithAvailabilities([availability])
            .WithId(list.Sections.First().Items.First().Id)
            .Create();
        var listReadModel = ToSimpleReadModel(list, store, item, null, null);

        Add(
            list,
            store,
            [item],
            [],
            [],
            listReadModel);
    }

    private void WithItemCategoryAndManufacturer()
    {
        IStore store = StoreMother.Initial().Create();
        var list = GetShoppingListContainingOneItem(store.Id, store.Sections.First().Id);
        IManufacturer manufacturer = ManufacturerMother.NotDeleted().Create();
        IItemCategory itemCategory = ItemCategoryMother.NotDeleted().Create();

        var availability = GetAvailabilityFrom(store);
        IItem item = new ItemBuilder()
            .WithItemCategoryId(itemCategory.Id)
            .WithManufacturerId(manufacturer.Id)
            .WithAvailabilities([availability])
            .WithId(list.Sections.First().Items.First().Id)
            .AsItem()
            .Create();
        var listReadModel = ToSimpleReadModel(list, store, item, itemCategory, manufacturer);

        Add(
            list,
            store,
            [item],
            [itemCategory],
            [manufacturer],
            listReadModel);
    }

    private void EmptyList()
    {
        IStore store = StoreMother.Initial().Create();
        var list = ShoppingListMother.NoSections().WithStoreId(store.Id).Create();
        var listReadModel = ToSimpleReadModel(list, store, null, null, null);

        Add(
            list,
            store,
            [],
            [],
            [],
            listReadModel);
    }

    private IShoppingList GetShoppingListContainingOneItem(StoreId storeId, SectionId sectionId)
    {
        var section = ShoppingListSectionMother.OneItemInBasket()
            .WithId(sectionId)
            .Create();
        return new ShoppingListBuilder()
            .WithStoreId(storeId)
            .WithSection(section)
            .Create();
    }

    private ItemAvailability GetAvailabilityFrom(IStore store)
    {
        return new ItemAvailabilityBuilder()
            .WithStoreId(store.Id)
            .WithDefaultSectionId(store.Sections.First().Id)
            .Create();
    }

    private static ShoppingListReadModel ToSimpleReadModel(IShoppingList list, IStore store, IItem? item,
        IItemCategory? itemCategory, IManufacturer? manufacturer)
    {
        var manufacturerReadModel = manufacturer == null
            ? null
            : new ManufacturerReadModel(
                manufacturer.Id,
                manufacturer.Name,
                manufacturer.IsDeleted);

        var itemCategoryReadModel = itemCategory == null
            ? null
            : new ItemCategoryReadModel(
                itemCategory.Id,
                itemCategory.Name,
                itemCategory.IsDeleted);

        var listItemReadModels = new List<ShoppingListItemReadModel>();
        if (item is not null)
        {
            var itemQuantityInPacket = item.ItemQuantity.InPacket;
            var quantityTypeInPacketReadModel = itemQuantityInPacket is null
                ? null
                : new QuantityTypeInPacketReadModel(itemQuantityInPacket.Type);

            var listItem = new ShoppingListItemReadModel(
                item.Id,
                null,
                item.Name,
                item.IsDeleted,
                item.Comment,
                item.IsTemporary,
                item.Availabilities.First().Price,
                new QuantityTypeReadModel(item.ItemQuantity.Type),
                itemQuantityInPacket?.Quantity,
                quantityTypeInPacketReadModel,
                itemCategoryReadModel,
                manufacturerReadModel,
                list.Sections.First().Items.First().IsInBasket,
                list.Sections.First().Items.First().Quantity,
                false);
            listItemReadModels.Add(listItem);
        }

        var sectionReadModels = list.Sections.Count != 0
            ? new ShoppingListSectionReadModel(
                list.Sections.First().Id,
                store.Sections.First().Name,
                store.Sections.First().SortingIndex,
                store.Sections.First().IsDefaultSection,
                listItemReadModels)
            : null;

        return new ShoppingListReadModel(
            list.Id,
            list.CompletionDate,
            new ShoppingListStoreReadModel(
                store.Id,
                store.Name),
            sectionReadModels is null ? [] : [sectionReadModels],
            list.ListDiscounts);
    }
}