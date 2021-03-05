using AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.Shared;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures
{
    public class StoreItemStoreFixture
    {
        private readonly CommonFixture commonFixture;

        public StoreItemStoreFixture(CommonFixture commonFixture)
        {
            this.commonFixture = commonFixture;
        }

        public IStoreItemStore Create()
        {
            var fixture = commonFixture.GetNewFixture();

            return fixture.Create<StoreItemStore>();
        }

        // todo: implement rest...
    }
}