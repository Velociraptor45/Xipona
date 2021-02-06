using AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Fixtures
{
    public class StoreItemSectionFixture
    {
        private readonly CommonFixture commonFixture;

        public StoreItemSectionFixture(CommonFixture commonFixture)
        {
            this.commonFixture = commonFixture;
        }

        public IEnumerable<IStoreItemSection> CreateMany(IEnumerable<int> ids)
        {
            if (ids is null)
                throw new ArgumentNullException(nameof(ids));

            return CreateMany(ids.Select(ids => new StoreItemSectionId(ids)));
        }

        public IEnumerable<IStoreItemSection> CreateMany(IEnumerable<StoreItemSectionId> ids)
        {
            if (ids is null)
                throw new ArgumentNullException(nameof(ids));

            foreach (var id in ids)
            {
                yield return Create(id);
            }
        }

        public IStoreItemSection Create(int id)
        {
            return Create(new StoreItemSectionId(id));
        }

        public IStoreItemSection Create(StoreItemSectionId id)
        {
            var fixture = commonFixture.GetNewFixture();

            fixture.Inject<StoreItemSectionId>(id);
            return fixture.Create<IStoreItemSection>();
        }
    }
}