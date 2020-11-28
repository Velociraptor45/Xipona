using AutoFixture;
using ShoppingList.Api.Domain.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Models.Fixtures
{
    public class StoreItemAvailabilityFixture
    {
        private readonly CommonFixture commonFixture;

        public StoreItemAvailabilityFixture()
        {
            commonFixture = new CommonFixture();
        }

        public StoreItemAvailability GetAvailability(StoreId storeId)
        {
            var fixture = commonFixture.GetNewFixture();
            fixture.Inject(storeId);
            return fixture.Create<StoreItemAvailability>();
        }

        public StoreItemAvailability GetAvailability(int storeId)
        {
            return GetAvailability(new StoreId(storeId));
        }

        public StoreItemAvailability GetAvailability()
        {
            return GetAvailability(commonFixture.NextInt());
        }
    }
}