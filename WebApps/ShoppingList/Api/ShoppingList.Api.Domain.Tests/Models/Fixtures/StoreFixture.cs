using AutoFixture;
using ShoppingList.Api.Core.Tests.AutoFixture;
using ShoppingList.Api.Domain.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Models.Fixtures
{
    public class StoreFixture
    {
        private readonly CommonFixture commonFixture;

        public StoreFixture(CommonFixture commonFixture)
        {
            this.commonFixture = commonFixture;
        }

        public Store GetStore(StoreId id = null, bool? isDeleted = null)
        {
            var fixture = commonFixture.GetNewFixture();

            if (id != null)
                fixture.ConstructorArgumentFor<Store, StoreId>("id", id);
            if (isDeleted.HasValue)
                fixture.ConstructorArgumentFor<Store, bool>("isDeleted", isDeleted.Value);

            return fixture.Create<Store>();
        }
    }
}