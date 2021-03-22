using AutoFixture;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ShoppingList.Api.Domain.TestKit.Common.Fixtures;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Fixtures;
using ShoppingList.Api.Domain.TestKit.Manufacturers.Fixtures;
using ShoppingList.Api.Domain.TestKit.Shared;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures
{
    public class ShoppingListItemFixture : IModelFixture<IShoppingListItem, ShoppingListItemDefinition>
    {
        private readonly CommonFixture commonFixture;
        private readonly ManufacturerFixture manufacturerFixture;
        private readonly ItemCategoryFixture itemCategoryFixture;

        public ShoppingListItemFixture(CommonFixture commonFixture)
        {
            this.commonFixture = commonFixture;
            manufacturerFixture = new ManufacturerFixture(commonFixture);
            itemCategoryFixture = new ItemCategoryFixture(commonFixture);
        }

        public IShoppingListItem Create()
        {
            return Create(new ShoppingListItemDefinition());
        }

        public IShoppingListItem Create(int id)
        {
            return Create(new ItemId(id));
        }

        public IShoppingListItem Create(ItemId id)
        {
            var definition = new ShoppingListItemDefinition
            {
                Id = id
            };
            return Create(definition);
        }

        public IShoppingListItem Create(ShoppingListItemDefinition definition)
        {
            var fixture = commonFixture.GetNewFixture();

            if (definition.Id != null)
                fixture.ConstructorArgumentFor<ShoppingListItem, ItemId>("id", definition.Id);
            if (definition.Name != null)
                fixture.ConstructorArgumentFor<ShoppingListItem, string>("name", definition.Name);
            if (definition.IsDeleted.HasValue)
                fixture.ConstructorArgumentFor<ShoppingListItem, bool>("isDeleted", definition.IsDeleted.Value);
            if (definition.Comment != null)
                fixture.ConstructorArgumentFor<ShoppingListItem, string>("comment", definition.Comment);
            if (definition.IsTemporary.HasValue)
                fixture.ConstructorArgumentFor<ShoppingListItem, bool>("isTemporary", definition.IsTemporary.Value);
            if (definition.PricePerQuantity.HasValue)
                fixture.ConstructorArgumentFor<ShoppingListItem, float>("pricePerQuantity", definition.PricePerQuantity.Value);
            if (definition.QuantityType.HasValue)
                fixture.ConstructorArgumentFor<ShoppingListItem, QuantityType>("quantityType", definition.QuantityType.Value);
            if (definition.QuantityInPacket.HasValue)
                fixture.ConstructorArgumentFor<ShoppingListItem, float>("quantityInPacket", definition.QuantityInPacket.Value);
            if (definition.QuantityTypeInPacket.HasValue)
                fixture.ConstructorArgumentFor<ShoppingListItem, QuantityTypeInPacket>("quantityTypeInPacket", definition.QuantityTypeInPacket.Value);
            if (definition.ItemCategory != null)
                fixture.ConstructorArgumentFor<ShoppingListItem, IItemCategory>("itemCategory", definition.ItemCategory);
            if (definition.Manufacturer != null)
                fixture.ConstructorArgumentFor<ShoppingListItem, IManufacturer>("manufacturer", definition.Manufacturer);
            if (definition.IsInBasket.HasValue)
                fixture.ConstructorArgumentFor<ShoppingListItem, bool>("isInBasket", definition.IsInBasket.Value);
            if (definition.Quantity.HasValue)
                fixture.ConstructorArgumentFor<ShoppingListItem, float>("quantity", definition.Quantity.Value);

            return fixture.Create<ShoppingListItem>();
        }

        public IEnumerable<IShoppingListItem> CreateMany(IEnumerable<ItemId> ids)
        {
            foreach (var id in ids)
                yield return Create(id);
        }

        public IShoppingListItem CreateUnique(IShoppingList shoppingList)
        {
            var usedItemIds = shoppingList.Items.Select(i => i.Id.Actual.Value);
            var itemId = commonFixture.NextInt(exclude: usedItemIds);
            return Create(itemId);
        }

        public IShoppingListItem CreateValid(ShoppingListItemDefinition baseDefinition)
        {
            baseDefinition.IsDeleted ??= false;

            if (baseDefinition.IsTemporary.HasValue)
            {
                if (baseDefinition.IsTemporary.Value)
                    EnrichAsTemporaryItem(baseDefinition);
                else
                    EnrichAsPermanentItem(baseDefinition);
            }
            else if (baseDefinition.ItemCategory != null)
            {
                EnrichAsPermanentItem(baseDefinition);
            }
            else if (commonFixture.NextBool())
            {
                EnrichAsPermanentItem(baseDefinition);
            }
            else
            {
                EnrichAsTemporaryItem(baseDefinition);
            }

            baseDefinition.QuantityType ??= commonFixture.ChooseRandom<QuantityType>();
            baseDefinition.QuantityTypeInPacket ??= commonFixture.ChooseRandom<QuantityTypeInPacket>();

            return Create(baseDefinition);
        }

        public IEnumerable<IShoppingListItem> CreateManyValid(int count = 3)
        {
            List<int> uniqueIds = commonFixture.NextUniqueInts(count).ToList();

            foreach (var id in uniqueIds)
            {
                var definition = ShoppingListItemDefinition.FromId(id);

                yield return Create(definition);
            }
        }

        public IEnumerable<IShoppingListItem> CreateManyValid(ShoppingListItemDefinition definition, int count = 3)
        {
            List<int> uniqueIds = commonFixture.NextUniqueInts(count).ToList();

            foreach (var id in uniqueIds)
            {
                var clone = definition.Clone();
                clone.Id = new ItemId(id);

                yield return Create(clone);
            }
        }

        public IEnumerable<IShoppingListItem> CreateManyValid(IEnumerable<ShoppingListItemDefinition> definitions)
        {
            var definitionsList = definitions.ToList();
            IEnumerable<int> existingIds = definitionsList
                .Where(d => d.Id != null && d.Id.IsActualId)
                .Select(d => d.Id.Actual.Value);
            List<int> uniqueIds = commonFixture
                .NextUniqueInts(definitionsList.Count, existingIds)
                .ToList();

            for (int i = 0; i < definitionsList.Count; i++)
            {
                int id = uniqueIds[i];
                var definition = definitionsList[i];
                definition.Id ??= new ItemId(id);
                yield return Create(definition);
            }
        }

        public IModelFixture<IShoppingListItem, ShoppingListItemDefinition> AsModelFixture()
        {
            return this;
        }

        private void EnrichAsTemporaryItem(ShoppingListItemDefinition definition)
        {
            definition.IsTemporary = true;
            definition.Manufacturer = null;
            definition.ItemCategory = null;
        }

        private void EnrichAsPermanentItem(ShoppingListItemDefinition definition)
        {
            definition.IsTemporary = true;
            definition.Manufacturer ??= manufacturerFixture.Create();
            definition.ItemCategory ??= itemCategoryFixture.GetItemCategory();
        }
    }
}