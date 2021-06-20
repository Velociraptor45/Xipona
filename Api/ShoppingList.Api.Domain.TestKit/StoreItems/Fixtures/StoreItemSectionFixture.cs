using AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures
{
    public class StoreItemSectionFixture
    {
        private readonly CommonFixture commonFixture;

        public StoreItemSectionFixture(CommonFixture commonFixture)
        {
            this.commonFixture = commonFixture;
        }

        public IEnumerable<IStoreSection> CreateMany(IEnumerable<int> ids)
        {
            if (ids is null)
                throw new ArgumentNullException(nameof(ids));

            return CreateMany(ids.Select(ids => new SectionId(ids)));
        }

        public IEnumerable<IStoreSection> CreateMany(IEnumerable<SectionId> ids)
        {
            if (ids is null)
                throw new ArgumentNullException(nameof(ids));

            foreach (var id in ids)
            {
                yield return Create(id);
            }
        }

        public IEnumerable<IStoreSection> CreateManyValid(int count = 3)
        {
            var uniqueIds = commonFixture.NextUniqueInts(count);
            return CreateMany(uniqueIds);
        }

        public IStoreSection Create(int id)
        {
            return Create(new SectionId(id));
        }

        public IStoreSection Create(SectionId id)
        {
            var fixture = commonFixture.GetNewFixture();

            fixture.Inject(id);
            return fixture.Create<IStoreSection>();
        }
    }
}