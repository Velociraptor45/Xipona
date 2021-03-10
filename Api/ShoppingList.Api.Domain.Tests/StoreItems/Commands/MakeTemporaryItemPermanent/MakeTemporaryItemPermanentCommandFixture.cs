using AutoFixture;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures.Commands;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.StoreItems.Commands.MakeTemporaryItemPermanent
{
    public class MakeTemporaryItemPermanentCommandFixture
    {
        private readonly CommonFixture commonFixture;
        private readonly ShortAvailabilityFixture shortAvailabilityFixture;

        public MakeTemporaryItemPermanentCommandFixture(CommonFixture commonFixture)
        {
            this.commonFixture = commonFixture;
            shortAvailabilityFixture = new ShortAvailabilityFixture(commonFixture);
        }

        public MakeTemporaryItemPermanentCommand Create(ManufacturerId manufacturerId,
            IEnumerable<ShortAvailability> availabilities = null)
        {
            var fixture = commonFixture.GetNewFixture();

            fixture.ConstructorArgumentFor<PermanentItem, ManufacturerId>("manufacturerId", manufacturerId);
            if (availabilities != null)
            {
                fixture.ConstructorArgumentFor<PermanentItem, IEnumerable<ShortAvailability>>(
                    "availabilities", availabilities);
            }

            return fixture.Create<MakeTemporaryItemPermanentCommand>();
        }

        public MakeTemporaryItemPermanentCommand CreateFromAvailabilities(ManufacturerId manufacturerId,
            IEnumerable<IStoreItemAvailability> availabilities = null)
        {
            var availabilitiesList = availabilities.ToList();

            List<ShortAvailability> shortAvailabilities = new List<ShortAvailability>();
            foreach (var availability in availabilitiesList)
            {
                var av = shortAvailabilityFixture.Create(availability);
                shortAvailabilities.Add(av);
            }

            return Create(manufacturerId, shortAvailabilities);
        }

        public MakeTemporaryItemPermanentCommand Create(ItemCategoryId itemCategoryId, ManufacturerId manufacturerId)
        {
            var fixture = commonFixture.GetNewFixture();

            fixture.ConstructorArgumentFor<PermanentItem, ManufacturerId>("manufacturerId", manufacturerId);
            fixture.ConstructorArgumentFor<PermanentItem, ItemCategoryId>("itemCategoryId", itemCategoryId);

            return fixture.Create<MakeTemporaryItemPermanentCommand>();
        }
    }
}