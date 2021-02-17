using AutoFixture;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateItem;
using ShoppingList.Api.Domain.TestKit.Shared;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.StoreItems.Commands.CreateItem
{
    public class CreateItemCommandFixture
    {
        private readonly CommonFixture commonFixture;

        public CreateItemCommandFixture(CommonFixture commonFixture)
        {
            this.commonFixture = commonFixture;
        }

        public CreateItemCommand GetCreateItemCommand(ManufacturerId manufacturerId)
        {
            var fixture = commonFixture.GetNewFixture();

            fixture.ConstructorArgumentFor<ItemCreation, ManufacturerId>("manufacturerId", manufacturerId);

            return fixture.Create<CreateItemCommand>();
        }

        public CreateItemCommand GetCreateItemCommand(ItemCategoryId itemCategoryId, ManufacturerId manufacturerId)
        {
            var fixture = commonFixture.GetNewFixture();

            fixture.ConstructorArgumentFor<ItemCreation, ManufacturerId>("manufacturerId", manufacturerId);
            fixture.ConstructorArgumentFor<ItemCreation, ItemCategoryId>("itemCategoryId", itemCategoryId);

            return fixture.Create<CreateItemCommand>();
        }
    }
}