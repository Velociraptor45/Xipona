using AutoFixture;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateTemporaryItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.Shared;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.StoreItems.Commands.CreateTemporaryItem
{
    public class CreateTemporaryItemCommandFixture
    {
        private readonly CommonFixture commonFixture;

        public CreateTemporaryItemCommandFixture(CommonFixture commonFixture)
        {
            this.commonFixture = commonFixture;
        }
    }
}