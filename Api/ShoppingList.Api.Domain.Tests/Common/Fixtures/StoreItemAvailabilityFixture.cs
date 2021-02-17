using AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Fixtures
{
    public class StoreItemAvailabilityFixture
    {
        private readonly CommonFixture commonFixture;

        public StoreItemAvailabilityFixture(CommonFixture commonFixture)
        {
            this.commonFixture = commonFixture;
        }

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
                yield return GetAvailability(new ShoppingListStoreId(id), section);
            }
        }

        public IStoreItemAvailability GetAvailability(ShoppingListStoreId storeId, IStoreItemSection section)
        {
            var fixture = commonFixture.GetNewFixture();
            fixture.Inject(storeId);
            fixture.Inject(section);
            return fixture.Create<StoreItemAvailability>();
        }

        public IStoreItemAvailability GetAvailability(IStoreItemSection section)
        {
            int storeId = commonFixture.NextInt();
            return GetAvailability(new ShoppingListStoreId(storeId), section);
        }

        public IStoreItemAvailability GetAvailability(ShoppingListStoreId storeId)
        {
            var fixture = commonFixture.GetNewFixture();
            fixture.Inject(storeId);
            return fixture.Create<StoreItemAvailability>();
        }

        public IStoreItemAvailability GetAvailability(int storeId)
        {
            return GetAvailability(new ShoppingListStoreId(storeId));
        }

        public IStoreItemAvailability GetAvailability()
        {
            return GetAvailability(commonFixture.NextInt());
        }
    }
}