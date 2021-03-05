using AutoFixture;
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

        public StoreItemAvailabilityFixture(CommonFixture commonFixture)
        {
            this.commonFixture = commonFixture;
        }

        // todo: entirely rewrite this
        // todo: make sure that the default section isn't different from the store's sections

        public IEnumerable<IStoreItemAvailability> GetAvailabilities(int count = 2)
        {
            if (count < 1)
                throw new ArgumentException($"{nameof(count)} mustn't be smaller than 1.");

            var uniqueStoreIds = commonFixture.NextUniqueInts(count);

            foreach (int uniqueStoreId in uniqueStoreIds)
            {
                yield return GetAvailability(uniqueStoreId);
            }
        }

        public IEnumerable<IStoreItemAvailability> GetAvailabilities(IEnumerable<IStoreItemSection> sections)
        {
            if (sections is null)
                throw new ArgumentNullException(nameof(sections));

            var sectionsList = sections.ToList();
            var uniqueIds = commonFixture.NextUniqueInts(sectionsList.Count).ToList();

            for (int i = 0; i < sectionsList.Count; i++)
            {
                IStoreItemSection section = sectionsList[i];
                int id = uniqueIds[i];
                yield return GetAvailability(new StoreItemStoreId(id), section);
            }
        }

        public IStoreItemAvailability GetAvailability(StoreItemStoreId storeId, IStoreItemSection section)
        {
            var fixture = commonFixture.GetNewFixture();
            fixture.Inject(storeId);
            fixture.Inject(section);
            return fixture.Create<StoreItemAvailability>();
        }

        public IStoreItemAvailability GetAvailability(IStoreItemSection section)
        {
            int storeId = commonFixture.NextInt();
            return GetAvailability(new StoreItemStoreId(storeId), section);
        }

        public IStoreItemAvailability GetAvailability(StoreItemStoreId storeId)
        {
            var fixture = commonFixture.GetNewFixture();
            fixture.Inject(storeId);
            return fixture.Create<StoreItemAvailability>();
        }

        public IStoreItemAvailability GetAvailability(int storeId)
        {
            return GetAvailability(new StoreItemStoreId(storeId));
        }

        public IStoreItemAvailability GetAvailability()
        {
            return GetAvailability(commonFixture.NextInt());
        }
    }
}