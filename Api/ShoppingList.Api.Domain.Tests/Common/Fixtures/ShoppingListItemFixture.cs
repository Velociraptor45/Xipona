using AutoFixture;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Fixtures
{
    public class ShoppingListItemFixture
    {
        private readonly CommonFixture commonFixture;

        public ShoppingListItemFixture(CommonFixture commonFixture)
        {
            this.commonFixture = commonFixture;
        }

        public ShoppingListItem GetShoppingListItemWithId(ShoppingListItemId id)
        {
            var fixture = commonFixture.GetNewFixture();
            fixture.Inject(id);
            return fixture.Create<ShoppingListItem>();
        }

        public ShoppingListItem GetShoppingListItemWithId(int id)
        {
            return GetShoppingListItemWithId(new ShoppingListItemId(id));
        }

        public ShoppingListItem GetShoppingListItemWithId(Guid id)
        {
            return GetShoppingListItemWithId(new ShoppingListItemId(id));
        }

        public ShoppingListItem GetShoppingListItemWithId()
        {
            return GetShoppingListItemWithId(commonFixture.NextInt());
        }

        public ShoppingListItem GetShoppingListItem(ShoppingListItemId id = null, bool? isInBasket = null,
            bool? isTemporary = null, bool? isDeleted = null)
        {
            var fixture = commonFixture.GetNewFixture();
            if (id != null)
                fixture.ConstructorArgumentFor<ShoppingListItem, ShoppingListItemId>("id", id);
            if (isInBasket.HasValue)
                fixture.ConstructorArgumentFor<ShoppingListItem, bool>("isInBasket", isInBasket.Value);
            if (isTemporary.HasValue)
                fixture.ConstructorArgumentFor<ShoppingListItem, bool>("isTemporary", isTemporary.Value);
            if (isDeleted.HasValue)
                fixture.ConstructorArgumentFor<ShoppingListItem, bool>("isDeleted", isDeleted.Value);

            return fixture.Create<ShoppingListItem>();
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

        public IEnumerable<IShoppingListItem> CreateSpecialAndRandom(ShoppingListItemId id, bool? isInBasket = null,
            bool? isTemporary = null, bool? isDeleted = null, int randomCount = 3)
        {
            IShoppingListItem special = GetShoppingListItem(id, isInBasket, isTemporary, isDeleted);
            IEnumerable<IShoppingListItem> randoms = CreateMany(randomCount);

            var result = randoms.ToList();
            result.Add(special);
            return result;
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
                yield return GetShoppingListItemWithId(id);
        }

        public IShoppingListItem CreateUnique(IShoppingList shoppingList)
        {
            var usedItemIds = shoppingList.Items.Select(i => i.Id.Actual.Value);
            var itemId = commonFixture.NextInt(exclude: usedItemIds);
            return GetShoppingListItemWithId(itemId);
        }
    }
}