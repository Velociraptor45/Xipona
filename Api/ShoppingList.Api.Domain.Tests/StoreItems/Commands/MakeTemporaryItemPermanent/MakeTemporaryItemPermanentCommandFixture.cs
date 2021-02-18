using AutoFixture;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.MakeTemporaryItemPermanent;
using ShoppingList.Api.Domain.TestKit.Shared;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.StoreItems.Commands.MakeTemporaryItemPermanent
{
    public class MakeTemporaryItemPermanentCommandFixture
    {
        private readonly CommonFixture commonFixture;

        public MakeTemporaryItemPermanentCommandFixture(CommonFixture commonFixture)
        {
            this.commonFixture = commonFixture;
        }

        public MakeTemporaryItemPermanentCommand GetCommand(ManufacturerId manufacturerId,
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

        public MakeTemporaryItemPermanentCommand GetCommand(ItemCategoryId itemCategoryId, ManufacturerId manufacturerId)
        {
            var fixture = commonFixture.GetNewFixture();

            fixture.ConstructorArgumentFor<PermanentItem, ManufacturerId>("manufacturerId", manufacturerId);
            fixture.ConstructorArgumentFor<PermanentItem, ItemCategoryId>("itemCategoryId", itemCategoryId);

            return fixture.Create<MakeTemporaryItemPermanentCommand>();
        }
    }
}