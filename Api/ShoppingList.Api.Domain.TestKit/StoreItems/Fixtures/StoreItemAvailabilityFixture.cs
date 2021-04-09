using AutoFixture;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.Stores.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures
{
    public class StoreItemAvailabilityFixture
    {
        private readonly CommonFixture commonFixture;
        private readonly StoreFixture storeFixture;

        public StoreItemAvailabilityFixture(CommonFixture commonFixture)
        {
            this.commonFixture = commonFixture;
            storeFixture = new StoreFixture(commonFixture);
        }

        public IStoreItemAvailability Create(StoreItemAvailabilityDefinition definition)
        {
            var fixture = commonFixture.GetNewFixture();

            if (definition.StoreId != null)
                fixture.ConstructorArgumentFor<StoreItemAvailability, StoreId>("storeId", definition.StoreId);
            if (definition.Price.HasValue)
                fixture.ConstructorArgumentFor<StoreItemAvailability, float>("price", definition.Price.Value);
            if (definition.DefaultSectionId != null)
                fixture.ConstructorArgumentFor<StoreItemAvailability, SectionId>("defaultSectionId", definition.DefaultSectionId);

            return fixture.Create<StoreItemAvailability>();
        }

        public IStoreItemAvailability CreateValid()
        {
            return Create(new StoreItemAvailabilityDefinition());
        }

        public IStoreItemAvailability CreateValidFor(IStore store)
        {
            var definition = new StoreItemAvailabilityDefinition
            {
                StoreId = store.Id,
                DefaultSectionId = commonFixture.ChooseRandom(store.Sections).Id
            };

            return Create(definition);
        }

        public IEnumerable<IStoreItemAvailability> CreateManyValid(int count = 3)
        {
            List<IStore> stores = storeFixture.CreateManyValid(count).ToList();
            foreach (var store in stores)
            {
                var defaultSection = commonFixture.ChooseRandom(store.Sections);

                var definition = new StoreItemAvailabilityDefinition
                {
                    StoreId = store.Id,
                    DefaultSectionId = defaultSection.Id
                };

                yield return Create(definition);
            }
        }

        public IEnumerable<IStoreItemAvailability> CreateManyValidWith(IStore store, int count = 3)
        {
            if (count <= 0)
                throw new ArgumentException($"{nameof(count)} must be greater than 0");

            var resultList = CreateValidFor(store).ToMonoList();
            resultList.AddRange(CreateManyValid(count - 1));

            return resultList;
        }
    }
}