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
using ShoppingList.Api.Domain.TestKit.ItemCategories.Fixtures;
using ShoppingList.Api.Domain.TestKit.Manufacturers.Fixtures;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures;
using ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures;
using ShoppingList.Api.Domain.TestKit.Stores.Fixtures;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ShoppingLists.Services.Conversion.ShoppingListReadModels
{
    public class ConvertAsyncTestData : IEnumerable<object[]>
    {
        private readonly CommonFixture commonFixture;
        private readonly ManufacturerFixture manufaturerFixture;
        private readonly ItemCategoryFixture itemCategoryFixture;
        private readonly StoreFixture storeFixture;
        private readonly StoreItemAvailabilityFixture storeItemAvailabilityFixture;
        private readonly StoreItemFixture storeItemFixture;
        private readonly ShoppingListFixture shoppingListFixture;
        private readonly ShoppingListSectionFixture shoppingListSectionFixture;
        private readonly ShoppingListItemFixture shoppingListItemFixture;

        public ConvertAsyncTestData()
        {
            commonFixture = new CommonFixture();
            manufaturerFixture = new ManufacturerFixture(commonFixture);
            itemCategoryFixture = new ItemCategoryFixture(commonFixture);
            storeFixture = new StoreFixture(commonFixture);

            storeItemAvailabilityFixture = new StoreItemAvailabilityFixture(commonFixture);
            storeItemFixture = new StoreItemFixture(storeItemAvailabilityFixture, commonFixture);

            shoppingListFixture = new ShoppingListFixture(commonFixture);
            shoppingListSectionFixture = new ShoppingListSectionFixture(commonFixture);
            shoppingListItemFixture = new ShoppingListItemFixture(commonFixture);
        }

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
            IStore store = storeFixture.CreateValid();
            var list = GetShoppingListContainingOneItem(store.Id, store.Sections.First().Id);
            IManufacturer manufacturer = manufaturerFixture.Create();

            var avavilabilityDef = new StoreItemAvailabilityDefinition
            {
                StoreId = store.Id,
                DefaultSectionId = store.Sections.First().Id
            };
            var availability = storeItemAvailabilityFixture.Create(avavilabilityDef);

            var itemDef = new StoreItemDefinition
            {
                Id = list.Sections.First().Items.First().Id,
                ItemCategoryId = null,
                ManufacturerId = manufacturer.Id,
                Availabilities = availability.ToMonoList()
            };

            IStoreItem item = storeItemFixture.Create(itemDef);
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
            IStore store = storeFixture.CreateValid();
            var list = GetShoppingListContainingOneItem(store.Id, store.Sections.First().Id);
            IItemCategory itemCategory = itemCategoryFixture.GetItemCategory();

            var avavilabilityDef = new StoreItemAvailabilityDefinition
            {
                StoreId = store.Id,
                DefaultSectionId = store.Sections.First().Id
            };
            var availability = storeItemAvailabilityFixture.Create(avavilabilityDef);

            var itemDef = new StoreItemDefinition
            {
                Id = list.Sections.First().Items.First().Id,
                ItemCategoryId = itemCategory.Id,
                ManufacturerId = null,
                Availabilities = availability.ToMonoList()
            };

            IStoreItem item = storeItemFixture.Create(itemDef);
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
            IStore store = storeFixture.CreateValid();
            var list = GetShoppingListContainingOneItem(store.Id, store.Sections.First().Id);

            var avavilabilityDef = new StoreItemAvailabilityDefinition
            {
                StoreId = store.Id,
                DefaultSectionId = store.Sections.First().Id
            };
            var availability = storeItemAvailabilityFixture.Create(avavilabilityDef);

            var itemDef = new StoreItemDefinition
            {
                Id = list.Sections.First().Items.First().Id,
                ItemCategoryId = null,
                ManufacturerId = null,
                Availabilities = availability.ToMonoList()
            };

            IStoreItem item = storeItemFixture.Create(itemDef);
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
            IStore store = storeFixture.CreateValid();
            var list = GetShoppingListContainingOneItem(store.Id, store.Sections.First().Id);
            IManufacturer manufacturer = manufaturerFixture.Create();
            IItemCategory itemCategory = itemCategoryFixture.GetItemCategory();

            var avavilabilityDef = new StoreItemAvailabilityDefinition
            {
                StoreId = store.Id,
                DefaultSectionId = store.Sections.First().Id
            };
            var availability = storeItemAvailabilityFixture.Create(avavilabilityDef);

            var itemDef = new StoreItemDefinition
            {
                Id = list.Sections.First().Items.First().Id,
                ItemCategoryId = itemCategory.Id,
                ManufacturerId = manufacturer.Id,
                Availabilities = availability.ToMonoList()
            };

            IStoreItem item = storeItemFixture.Create(itemDef);
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
            IStore store = storeFixture.CreateValid();
            var listDef = new ShoppingListDefinition
            {
                StoreId = store.Id,
                Sections = Enumerable.Empty<IShoppingListSection>()
            };
            var list = shoppingListFixture.Create(listDef);
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
            var sectionDef = new ShoppingListSectionDefinition
            {
                Id = sectionId,
                Items = shoppingListItemFixture.AsModelFixture().CreateValid().ToMonoList(),
            };
            var section = shoppingListSectionFixture.CreateValid(sectionDef);

            var listDef = new ShoppingListDefinition
            {
                StoreId = storeId,
                Sections = section.ToMonoList()
            };
            return shoppingListFixture.CreateValid(listDef);
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
}