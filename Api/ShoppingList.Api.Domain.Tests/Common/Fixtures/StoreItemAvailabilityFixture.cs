using AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Fixtures
{
    public class StoreItemAvailabilityFixture
    {
        private readonly CommonFixture commonFixture;

        public StoreItemAvailabilityFixture(CommonFixture commonFixture)
        {
            this.commonFixture = commonFixture;
        }

        public IEnumerable<StoreItemAvailability> GetAvailabilities(int count = 2)
        {
            if (count < 1)
                throw new ArgumentException($"{nameof(count)} mustn't be smaller than 1.");

            var uniqueStoreIds = commonFixture.NextUniqueInts(count);

            foreach (int uniqueStoreId in uniqueStoreIds)
            {
                yield return GetAvailability(uniqueStoreId);
            }
        }

        public StoreItemAvailability GetAvailability(StoreId storeId)
        {
            var fixture = commonFixture.GetNewFixture();
            fixture.Inject(storeId);
            return fixture.Create<StoreItemAvailability>();
        }

        public StoreItemAvailability GetAvailability(int storeId)
        {
            return GetAvailability(new StoreId(storeId));
        }

        public StoreItemAvailability GetAvailability()
        {
            return GetAvailability(commonFixture.NextInt());
        }
    }
}