using AutoFixture;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Fixtures
{
    public class StoreFixture
    {
        private readonly CommonFixture commonFixture;

        public StoreFixture(CommonFixture commonFixture)
        {
            this.commonFixture = commonFixture;
        }

        public IStore GetStore(StoreId id = null, bool? isDeleted = null)
        {
            var fixture = commonFixture.GetNewFixture();

            if (id != null)
                fixture.ConstructorArgumentFor<Store, StoreId>("id", id);
            if (isDeleted.HasValue)
                fixture.ConstructorArgumentFor<Store, bool>("isDeleted", isDeleted.Value);

            return fixture.Create<Store>();
        }

        public IEnumerable<IStore> GetStores(int count = 3, bool? isDeleted = null)
        {
            if (count < 1)
                throw new ArgumentException($"{nameof(count)} mustn't be smaller than 1.");

            var uniqueStoreIds = commonFixture.NextUniqueInts(count);

            foreach (int uniqueStoreId in uniqueStoreIds)
            {
                yield return GetStore(new StoreId(uniqueStoreId), isDeleted);
            }
        }
    }
}