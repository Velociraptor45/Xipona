using AutoFixture;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ShoppingList.Api.Domain.TestKit.Shared;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures
{
    public class ShoppingListItemFixture
    {
        private readonly CommonFixture commonFixture;

        public ShoppingListItemFixture(CommonFixture commonFixture)
        {
            this.commonFixture = commonFixture;
        }

        public IShoppingListItem Create()
        {
            return Create(new ShoppingListItemGenerationDefinition());
        }

        public IShoppingListItem Create(int id)
        {
            return Create(new ShoppingListItemId(id));
        }

        public IShoppingListItem Create(ShoppingListItemId id)
        {
            var definition = new ShoppingListItemGenerationDefinition
            {
                Id = id
            };
            return Create(definition);
        }

        public IShoppingListItem Create(ShoppingListItemGenerationDefinition definition)
        {
            var fixture = commonFixture.GetNewFixture();

            if (definition.Id != null)
                fixture.ConstructorArgumentFor<ShoppingListItem, ShoppingListItemId>("id", definition.Id);
            if (definition.Name != null)
                fixture.ConstructorArgumentFor<ShoppingListItem, string>("name", definition.Name);
            //todo rest

            return fixture.Create<ShoppingListItem>();
        }

        public IEnumerable<IShoppingListItem> CreateMany(int count)
        {
            IEnumerable<int> uniqueIds = commonFixture.NextUniqueInts(count);
            IEnumerable<ShoppingListItemId> uniqueSectionIds = uniqueIds.Select(id => new ShoppingListItemId(id));
            return CreateMany(uniqueSectionIds);
        }

        public IEnumerable<IShoppingListItem> CreateMany(IEnumerable<ShoppingListItemId> ids)
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

        public IEnumerable<IShoppingListItem> CreateSpecialAndRandom(ShoppingListItemGenerationDefinition definition,
            int randomCount = 3)
        {
            IShoppingListItem special = Create(definition);
            IEnumerable<IShoppingListItem> randoms = CreateMany(randomCount);

            var result = randoms.ToList();
            result.Add(special);
            return result;
        }
    }
}