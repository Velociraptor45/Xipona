using AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Models.Fixtures;
using ShoppingList.Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Models
{
    public class ShoppingListItemFixture
    {
        private readonly CommonFixture commonFixture;
        private readonly StoreItemAvailabilityFixture storeItemAvailabilityFixture;

        public ShoppingListItemFixture()
        {
            commonFixture = new CommonFixture();
            storeItemAvailabilityFixture = new StoreItemAvailabilityFixture();
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

        public ShoppingListItem GetShoppingListItem()
        {
            return GetShoppingListItemWithId(commonFixture.NextInt());
        }

        public StoreItem GetStoreItemWithId(StoreItemId id)
        {
            var fixture = commonFixture.GetNewFixture();
            fixture.Inject(id);
            return fixture.Create<StoreItem>();
        }

        public StoreItem GetStoreItem(StoreItemId id, int availabilityCount, IEnumerable<int> requiredStoreIds = null)
        {
            List<StoreItemAvailability> availabilities = new List<StoreItemAvailability>();
            List<int> requiredStoreIdList = requiredStoreIds?.ToList() ?? new List<int>();

            for (int i = 0; i < availabilityCount; i++)
            {
                if (i < requiredStoreIdList.Count)
                {
                    int storeId = requiredStoreIdList[i];
                    availabilities.Add(storeItemAvailabilityFixture.GetAvailability(storeId));
                    continue;
                }

                availabilities.Add(storeItemAvailabilityFixture.GetAvailability());
            }

            var fixture = commonFixture.GetNewFixture();
            fixture.Inject(id);
            fixture.Inject(availabilities);
            return fixture.Create<StoreItem>();
        }

        public StoreItem GetStoreItem(int availabilityCount, IEnumerable<int> requiredStoreIds = null)
        {
            return GetStoreItem(new StoreItemId(commonFixture.NextInt()), availabilityCount, requiredStoreIds);
        }

        public StoreItem GetStoreItem(int storeItemId, int availabilityCount, IEnumerable<int> requiredStoreIds = null)
        {
            return GetStoreItem(new StoreItemId(storeItemId), availabilityCount, requiredStoreIds);
        }
    }
}