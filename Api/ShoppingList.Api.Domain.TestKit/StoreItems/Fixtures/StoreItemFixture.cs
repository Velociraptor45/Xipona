using AutoFixture;
using ProjectHermes.ShoppingList.Api.Core.Tests;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.Shared;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures
{
    public class StoreItemFixture
    {
        private readonly StoreItemAvailabilityFixture storeItemAvailabilityFixture;
        private readonly CommonFixture commonFixture;
        private readonly StoreItemStoreFixture storeItemStoreFixture;

        public StoreItemFixture(StoreItemAvailabilityFixture storeItemAvailabilityFixture, CommonFixture commonFixture)
        {
            this.storeItemAvailabilityFixture = storeItemAvailabilityFixture;
            this.commonFixture = commonFixture;
            storeItemStoreFixture = new StoreItemStoreFixture(commonFixture);
        }

        public IStoreItem GetStoreItem(StoreItemId id = null, int availabilityCount = 3,
            IEnumerable<IStoreItemAvailability> additionalAvailabilities = null,
            bool? isTemporary = null, bool? isDeleted = null)
        {
            var allAvailabilities = additionalAvailabilities?.ToList() ?? new List<IStoreItemAvailability>();
            var additionalStoreIds = allAvailabilities.Select(av => av.Store.Id.Value);
            var uniqueStoreItemAvailabilities = GetUniqueStoreItemAvailabilities(availabilityCount, additionalStoreIds);
            allAvailabilities.AddRange(uniqueStoreItemAvailabilities);
            allAvailabilities.Shuffle();

            var fixture = commonFixture.GetNewFixture();
            fixture.Inject(allAvailabilities.AsEnumerable());
            if (id != null)
                fixture.Inject(id);
            if (isTemporary.HasValue)
                fixture.ConstructorArgumentFor<StoreItem, bool>("isTemporary", isTemporary.Value);
            if (isDeleted.HasValue)
                fixture.ConstructorArgumentFor<StoreItem, bool>("isDeleted", isDeleted.Value);
            return fixture.Create<StoreItem>();
        }

        public IStoreItem Create(StoreItemDefinition definition)
        {
            var fixture = commonFixture.GetNewFixture();

            if (definition.Id != null)
                fixture.ConstructorArgumentFor<StoreItem, StoreItemId>("id", definition.Id);
            if (definition.IsDeleted.HasValue)
                fixture.ConstructorArgumentFor<StoreItem, bool>("isDeleted", definition.IsDeleted.Value);
            if (definition.IsTemporary.HasValue)
                fixture.ConstructorArgumentFor<StoreItem, bool>("isTemporary", definition.IsTemporary.Value);
            if (definition.Availabilities != null)
                fixture.ConstructorArgumentFor<StoreItem, IEnumerable<IStoreItemAvailability>>(
                    "availabilities", definition.Availabilities);

            return fixture.Create<StoreItem>();
        }

        public IStoreItem CreateValid()
        {
            return CreateValid(new StoreItemDefinition());
        }

        public IStoreItem CreateValid(StoreItemDefinition baseDefinition)
        {
            baseDefinition.IsTemporary ??= false;
            baseDefinition.IsDeleted ??= false;
            baseDefinition.Availabilities ??= storeItemAvailabilityFixture.CreateManyValid();

            return Create(baseDefinition);
        }

        public IStoreItem CreateValidFor(IShoppingList shoppingList)
        {
            var storeDefinition = StoreItemStoreDefinition.FromId(shoppingList.Store.Id.Value);
            var store = storeItemStoreFixture.Create(storeDefinition);

            var availabilities = storeItemAvailabilityFixture.CreateManyValidFor(store, 4);
            var definition = new StoreItemDefinition
            {
                Availabilities = availabilities
            };

            return CreateValid(definition);
        }

        public IEnumerable<IStoreItem> CreateMany(IEnumerable<StoreItemDefinition> definitions)
        {
            var existingIds = definitions
                .Where(def => def.Id != null)
                .Select(d => d.Id.Actual.Value); // todo implement this for offline ids
            var newIdCount = definitions.ToList().Count - existingIds.ToList().Count;
            var newIds = commonFixture.NextUniqueInts(newIdCount, exclude: existingIds).ToList();

            foreach (var definition in definitions)
            {
                if (definition.Id == null)
                {
                    definition.Id = new StoreItemId(newIds.First());
                    newIds.RemoveAt(0);
                }

                yield return Create(definition);
            }
        }

        private IEnumerable<IStoreItemAvailability> GetUniqueStoreItemAvailabilities(int count,
            IEnumerable<int> exclude = null)
        {
            List<int> storeIds = commonFixture.NextUniqueInts(count, exclude).ToList();
            List<IStoreItemAvailability> availabilities = new List<IStoreItemAvailability>();

            foreach (var storeId in storeIds)
            {
                var availability = storeItemAvailabilityFixture.GetAvailability(storeId);
                availabilities.Add(availability);
            }
            return availabilities;
        }
    }
}