using AutoFixture;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.Shared;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures.Commands
{
    public class ShortAvailabilityFixture
    {
        private readonly CommonFixture commonFixture;

        public ShortAvailabilityFixture(CommonFixture commonFixture)
        {
            this.commonFixture = commonFixture;
        }

        public ShortAvailability Create(IStoreItemAvailability availability)
        {
            var fixture = commonFixture.GetNewFixture();

            fixture.ConstructorArgumentFor<ShortAvailability, StoreItemStoreId>("storeId", availability.StoreId.Id);
            fixture.ConstructorArgumentFor<ShortAvailability, float>("price", availability.Price);
            fixture.ConstructorArgumentFor<ShortAvailability, StoreItemSectionId>("storeItemSectionId", availability.DefaultSectionId.Id);

            return fixture.Create<ShortAvailability>();
        }
    }
}