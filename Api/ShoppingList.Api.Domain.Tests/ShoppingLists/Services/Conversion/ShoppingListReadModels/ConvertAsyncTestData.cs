using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProjectHermes.ShoppingList.Api.Core.Attributes;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.ActiveShoppingListByStoreId;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.AllQuantityTypes;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.AllQuantityTypesInPacket;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Models;
using ShoppingList.Api.Domain.TestKit.Manufacturers.Models;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using ShoppingList.Api.Domain.TestKit.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ShoppingLists.Services.Conversion.ShoppingListReadModels;

public class ConvertAsyncTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return NoItemCategory();
        yield return NoManufacturer();
        yield return NeitherItemCategoryNorManufacturer();
        yield return WithItemCategoryAndManufacturer();
        yield return EmptyList();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private object[] NoItemCategory()
    {
        IStore store = StoreMother.Initial().Create();
        var list = GetShoppingListContainingOneItem(store.Id, store.Sections.First().Id);
        IManufacturer manufacturer = ManufacturerMother.NotDeleted().Create();

        var availability = GetAvailabilityFrom(store);

        IStoreItem item = new StoreItemBuilder()
            .WithoutItemCategoryId()
            .WithManufacturerId(manufacturer.Id)
            .WithId(list.Sections.First().Items.First().Id)
            .WithAvailabilities(availability.ToMonoList())
            .AsItem()
            .Create();
        var listReadModel = ToSimpleReadModel(list, store, item, null, manufacturer);

        return new object[]
        {
            list,
            store,
            item.ToMonoList(),
            Enumerable.Empty<IItemCategory>(),
            manufacturer.ToMonoList(),
            listReadModel
        };
    }

    private object[] NoManufacturer()
    {
        IStore store = StoreMother.Initial().Create();
        var list = GetShoppingListContainingOneItem(store.Id, store.Sections.First().Id);
        IItemCategory itemCategory = ItemCategoryMother.NotDeleted().Create();

        var availability = GetAvailabilityFrom(store);
        IStoreItem item = new StoreItemBuilder()
            .WithItemCategoryId(itemCategory.Id)
            .WithoutManufacturerId()
            .WithAvailabilities(availability.ToMonoList())
            .WithId(list.Sections.First().Items.First().Id)
            .AsItem()
            .Create();

        var listReadModel = ToSimpleReadModel(list, store, item, itemCategory, null);

        return new object[]
        {
            list,
            store,
            item.ToMonoList(),
            itemCategory.ToMonoList(),
            Enumerable.Empty<IManufacturer>(),
            listReadModel
        };
    }

    private object[] NeitherItemCategoryNorManufacturer()
    {
        IStore store = StoreMother.Initial().Create();
        var list = GetShoppingListContainingOneItem(store.Id, store.Sections.First().Id);

        var availability = GetAvailabilityFrom(store);
        IStoreItem item = StoreItemMother.InitialTemporary()
            .WithAvailabilities(availability.ToMonoList())
            .WithId(list.Sections.First().Items.First().Id)
            .Create();
        var listReadModel = ToSimpleReadModel(list, store, item, null, null);

        return new object[]
        {
            list,
            store,
            item.ToMonoList(),
            Enumerable.Empty<IItemCategory>(),
            Enumerable.Empty<IManufacturer>(),
            listReadModel
        };
    }

    private object[] WithItemCategoryAndManufacturer()
    {
        IStore store = StoreMother.Initial().Create();
        var list = GetShoppingListContainingOneItem(store.Id, store.Sections.First().Id);
        IManufacturer manufacturer = ManufacturerMother.NotDeleted().Create();
        IItemCategory itemCategory = ItemCategoryMother.NotDeleted().Create();

        var availability = GetAvailabilityFrom(store);
        IStoreItem item = new StoreItemBuilder()
            .WithItemCategoryId(itemCategory.Id)
            .WithManufacturerId(manufacturer.Id)
            .WithAvailabilities(availability.ToMonoList())
            .WithId(list.Sections.First().Items.First().Id)
            .AsItem()
            .Create();
        var listReadModel = ToSimpleReadModel(list, store, item, itemCategory, manufacturer);

        return new object[]
        {
            list,
            store,
            item.ToMonoList(),
            itemCategory.ToMonoList(),
            manufacturer.ToMonoList(),
            listReadModel
        };
    }

    private object[] EmptyList()
    {
        IStore store = StoreMother.Initial().Create();
        var list = ShoppingListMother.NoSections().WithStoreId(store.Id).Create();
        var listReadModel = ToSimpleReadModel(list, store, null, null, null);

        return new object[]
        {
            list,
            store,
            Enumerable.Empty<IStoreItem>(),
            Enumerable.Empty<IItemCategory>(),
            Enumerable.Empty<IManufacturer>(),
            listReadModel
        };
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

    private IStoreItemAvailability GetAvailabilityFrom(IStore store)
    {
        return new StoreItemAvailabilityBuilder()
            .WithStoreId(store.Id)
            .WithDefaultSectionId(store.Sections.First().Id)
            .Create();
    }

    private ShoppingListReadModel ToSimpleReadModel(IShoppingList list, IStore store, IStoreItem item,
        IItemCategory itemCategory, IManufacturer manufacturer)
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

        var sectionReadModels = list.Sections.Any()
            ? new ShoppingListSectionReadModel(
                list.Sections.First().Id,
                store.Sections.First().Name,
                store.Sections.First().SortingIndex,
                store.Sections.First().IsDefaultSection,
                new List<ShoppingListItemReadModel>
                {
                    new ShoppingListItemReadModel(
                        item.Id,
                        null,
                        item.Name,
                        item.IsDeleted,
                        item.Comment,
                        item.IsTemporary,
                        item.Availabilities.First().Price,
                        new QuantityTypeReadModel(
                            (int)item.QuantityType,
                            item.QuantityType.ToString(),
                            item.QuantityType.GetAttribute<DefaultQuantityAttribute>().DefaultQuantity,
                            item.QuantityType.GetAttribute<PriceLabelAttribute>().PriceLabel,
                            item.QuantityType.GetAttribute<QuantityLabelAttribute>().QuantityLabel,
                            item.QuantityType.GetAttribute<QuantityNormalizerAttribute>().Value),
                        item.QuantityInPacket,
                        new QuantityTypeInPacketReadModel(
                            (int)item.QuantityTypeInPacket,
                            item.QuantityTypeInPacket.ToString(),
                            item.QuantityTypeInPacket.GetAttribute<QuantityLabelAttribute>().QuantityLabel),
                        itemCategoryReadModel,
                        manufacturerReadModel,
                        list.Sections.First().Items.First().IsInBasket,
                        list.Sections.First().Items.First().Quantity)
                })
            : null;

        return new ShoppingListReadModel(
            list.Id,
            list.CompletionDate,
            new ShoppingListStoreReadModel(
                store.Id,
                store.Name),
            sectionReadModels != null
                ? sectionReadModels.ToMonoList()
                : Enumerable.Empty<ShoppingListSectionReadModel>());
    }
}