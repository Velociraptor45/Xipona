using AutoFixture;
using ShoppingList.Api.Core.Tests;
using ShoppingList.Api.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Models.Fixtures
{
    public class StoreItemFixture
    {
        private readonly StoreItemAvailabilityFixture storeItemAvailabilityFixture;
        private readonly CommonFixture commonFixture;

        public StoreItemFixture(StoreItemAvailabilityFixture storeItemAvailabilityFixture, CommonFixture commonFixture)
        {
            this.storeItemAvailabilityFixture = storeItemAvailabilityFixture;
            this.commonFixture = commonFixture;
        }

        public StoreItem GetStoreItem(int availabilityCount = 3,
            IEnumerable<StoreItemAvailability> additionalAvailabilities = null)
        {
            var storeItemId = new StoreItemId(commonFixture.NextInt());
            return GetStoreItem(storeItemId, availabilityCount, additionalAvailabilities);
        }

        public StoreItem GetStoreItem(StoreItemId id, int availabilityCount = 3,
            IEnumerable<StoreItemAvailability> additionalAvailabilities = null)
        {
            var allAvailabilities = additionalAvailabilities?.ToList() ?? new List<StoreItemAvailability>();
            var additionalStoreIds = allAvailabilities.Select(av => av.StoreId.Value);
            var uniqueStoreItemAvailabilities = GetUniqueStoreItemAvailabilities(availabilityCount, additionalStoreIds);
            allAvailabilities.AddRange(uniqueStoreItemAvailabilities);
            allAvailabilities.Shuffle();

            var fixture = commonFixture.GetNewFixture();
            fixture.Inject(id);
            fixture.Inject(allAvailabilities.AsEnumerable());
            return fixture.Create<StoreItem>();
        }

        private IEnumerable<StoreItemAvailability> GetUniqueStoreItemAvailabilities(int count, IEnumerable<int> exclude)
        {
            List<int> storeIds = commonFixture.NextUniqueInts(count, exclude).ToList();
            List<StoreItemAvailability> availabilities = new List<StoreItemAvailability>();

            foreach (var storeId in storeIds)
            {
                var availability = storeItemAvailabilityFixture.GetAvailability(storeId);
                availabilities.Add(availability);
            }
            return availabilities;
        }
    }
}