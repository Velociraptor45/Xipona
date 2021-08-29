using AutoFixture;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.Common.Fixtures;
using ShoppingList.Api.Domain.TestKit.Shared;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures
{
    public class ShoppingListItemFixture : IModelFixture<IShoppingListItem, ShoppingListItemDefinition>
    {
        private readonly CommonFixture commonFixture;

        public ShoppingListItemFixture(CommonFixture commonFixture)
        {
            this.commonFixture = commonFixture;
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
            if (definition.ItemCategoryId != null)
                fixture.ConstructorArgumentFor<ShoppingListItem, ItemCategoryId>("itemCategory", definition.ItemCategoryId);
            if (definition.ManufacturerId != null)
                fixture.ConstructorArgumentFor<ShoppingListItem, ManufacturerId>("manufacturer", definition.ManufacturerId);
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
            var usedItemIds = shoppingList.Items.Select(i => i.Id.Value);
            var itemId = commonFixture.NextInt(exclude: usedItemIds);
            return Create(itemId);
        }

        public IShoppingListItem CreateValid(ShoppingListItemDefinition baseDefinition)
        {
            if (baseDefinition.ItemCategoryId != null)
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
                .Select(d => d.Id.Value);
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

        public IShoppingListItem CreateValidWithBasketStatus(bool isInBasket)
        {
            return CreateValid(ShoppingListItemDefinition.FromIsInBasket(isInBasket));
        }

        public IModelFixture<IShoppingListItem, ShoppingListItemDefinition> AsModelFixture()
        {
            return this;
        }

        private void EnrichAsTemporaryItem(ShoppingListItemDefinition definition)
        {
            definition.ManufacturerId = null;
            definition.ItemCategoryId = null;
        }

        private void EnrichAsPermanentItem(ShoppingListItemDefinition definition)
        {
            definition.ManufacturerId ??= new ManufacturerId(commonFixture.NextInt());
            definition.ItemCategoryId ??= new ItemCategoryId(commonFixture.NextInt());
        }
    }
}