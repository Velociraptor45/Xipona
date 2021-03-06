using AutoFixture;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.Shared;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures
{
    public class StoreItemStoreFixture
    {
        private readonly CommonFixture commonFixture;
        private readonly StoreItemSectionFixture storeItemSectionFixture;

        public StoreItemStoreFixture(CommonFixture commonFixture)
        {
            this.commonFixture = commonFixture;
            storeItemSectionFixture = new StoreItemSectionFixture(commonFixture);
        }

        public IStoreItemStore Create()
        {
            var fixture = commonFixture.GetNewFixture();

            return fixture.Create<StoreItemStore>();
        }

        public IStoreItemStore Create(StoreItemStoreDefinition definition)
        {
            var fixture = commonFixture.GetNewFixture();

            if (definition.Id != null)
                fixture.ConstructorArgumentFor<StoreItemStore, StoreItemStoreId>("id", definition.Id);
            if (definition.Name != null)
                fixture.ConstructorArgumentFor<StoreItemStore, string>("name", definition.Name);
            if (definition.Sections != null)
                fixture.ConstructorArgumentFor<StoreItemStore, IEnumerable<IStoreItemSection>>("sections", definition.Sections);

            return fixture.Create<StoreItemStore>();
        }

        public IStoreItemStore CreateValid()
        {
            var definition = new StoreItemStoreDefinition
            {
                Sections = storeItemSectionFixture.CreateManyValid()
            };

            return Create(definition);
        }

        public IEnumerable<IStoreItemStore> CreateManyValid(int count = 3, IEnumerable<StoreItemStoreId> excludedStoreIds = null)
        {
            IEnumerable<int> excludedIds = excludedStoreIds?.Select(id => id.Value) ?? Enumerable.Empty<int>();

            var uniqueIds = commonFixture.NextUniqueInts(count, excludedIds).ToList();
            foreach (var id in uniqueIds)
            {
                var definition = new StoreItemStoreDefinition
                {
                    Id = new StoreItemStoreId(id),
                    Sections = storeItemSectionFixture.CreateManyValid()
                };

                yield return Create(definition);
            }
        }
    }
}