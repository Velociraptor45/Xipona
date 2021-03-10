using AutoFixture;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateTemporaryItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures.Commands;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.StoreItems.Commands.CreateTemporaryItem
{
    public class CreateTemporaryItemCommandFixture
    {
        private readonly CommonFixture commonFixture;
        private readonly ShortAvailabilityFixture shortAvailabilityFixture;

        public CreateTemporaryItemCommandFixture(CommonFixture commonFixture)
        {
            this.commonFixture = commonFixture;
            shortAvailabilityFixture = new ShortAvailabilityFixture(commonFixture);
        }

        public CreateTemporaryItemCommand Create(IStoreItemAvailability availability)
        {
            var fixture = commonFixture.GetNewFixture();

            var shortAv = shortAvailabilityFixture.Create(availability);
            fixture.ConstructorArgumentFor<TemporaryItemCreation, ShortAvailability>("availability", shortAv);

            return fixture.Create<CreateTemporaryItemCommand>();
        }
    }
}