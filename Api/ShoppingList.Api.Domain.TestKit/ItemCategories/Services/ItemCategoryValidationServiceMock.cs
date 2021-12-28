using Moq;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services;
using System.Threading;

namespace ShoppingList.Api.Domain.TestKit.ItemCategories.Services
{
    public class ItemCategoryValidationServiceMock : Mock<IItemCategoryValidationService>
    {
        public ItemCategoryValidationServiceMock(MockBehavior behavior) : base(behavior)
        {
        }

        public void VerifyValidateAsyncOnce(ItemCategoryId itemCategoryId)
        {
            Verify(i => i.ValidateAsync(
                    itemCategoryId,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}