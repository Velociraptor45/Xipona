using AutoFixture;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services;
using System.Threading;

namespace ShoppingList.Api.Domain.TestKit.ItemCategories.Services
{
    public class ItemCategoryValidationServiceMock
    {
        private readonly Mock<IItemCategoryValidationService> mock;

        public ItemCategoryValidationServiceMock(Mock<IItemCategoryValidationService> mock)
        {
            this.mock = mock;
        }

        public ItemCategoryValidationServiceMock(Fixture fixture)
        {
            mock = fixture.Freeze<Mock<IItemCategoryValidationService>>();
        }

        public void VerifyValidateAsyncOnce(ItemCategoryId itemCategoryId)
        {
            mock.Verify(i => i.ValidateAsync(
                    itemCategoryId,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}