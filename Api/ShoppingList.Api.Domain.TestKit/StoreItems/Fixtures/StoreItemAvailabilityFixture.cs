using AutoFixture;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures
{
    public class StoreItemAvailabilityFixture
    {
        private readonly CommonFixture commonFixture;
        private readonly StoreItemStoreFixture storeItemStoreFixture;

        public StoreItemAvailabilityFixture(CommonFixture commonFixture)
        {
            this.commonFixture = commonFixture;
            storeItemStoreFixture = new StoreItemStoreFixture(commonFixture);
        }

        public IStoreItemAvailability Create(StoreItemAvailabilityDefinition definition)
        {
            var fixture = commonFixture.GetNewFixture();

            if (definition.Store != null)
                fixture.ConstructorArgumentFor<StoreItemAvailability, IStoreItemStore>("store", definition.Store);
            if (definition.Price.HasValue)
                fixture.ConstructorArgumentFor<StoreItemAvailability, float>("price", definition.Price.Value);
            if (definition.DefaultSectionId != null)
                fixture.ConstructorArgumentFor<StoreItemAvailability, StoreItemSectionId>("defaultSectionId", definition.DefaultSectionId);

            return fixture.Create<StoreItemAvailability>();
        }

        public IStoreItemAvailability CreateValid()
        {
            var store = storeItemStoreFixture.CreateValid();
            var defaultSection = commonFixture.ChooseRandom(store.Sections);

            var definition = new StoreItemAvailabilityDefinition
            {
                Store = store,
                DefaultSectionId = defaultSection.Id
            };
            return Create(definition);
        }

        public IStoreItemAvailability CreateValidFor(IStoreItemStore store)
        {
            var definition = new StoreItemAvailabilityDefinition
            {
                Store = store,
                DefaultSectionId = commonFixture.ChooseRandom(store.Sections).Id
            };

            return Create(definition);
        }

        public IEnumerable<IStoreItemAvailability> CreateManyValid(int count = 3, IEnumerable<StoreItemStoreId> excludedStoreIds = null)
        {
            List<IStoreItemStore> stores = storeItemStoreFixture.CreateManyValid(count, excludedStoreIds).ToList();
            foreach (var store in stores)
            {
                var defaultSection = commonFixture.ChooseRandom(store.Sections);

                var definition = new StoreItemAvailabilityDefinition
                {
                    Store = store,
                    DefaultSectionId = defaultSection.Id
                };

                yield return Create(definition);
            }
        }

        public IEnumerable<IStoreItemAvailability> CreateManyValidFor(IStoreItemStore store, int count = 3)
        {
            if (count <= 0)
                throw new ArgumentException($"{nameof(count)} must be greater than 0");

            var resultList = CreateValidFor(store).ToMonoList();
            resultList.AddRange(CreateManyValid(count - 1));

            return resultList;
        }
    }
}