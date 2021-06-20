using Moq;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;

namespace ShoppingList.Api.Domain.TestKit.ItemCategories.Models
{
    public class ItemCategoryMock : Mock<IItemCategory>
    {
        public ItemCategoryMock(IItemCategory itemCategory)
        {
            SetupId(itemCategory.Id);
        }

        public void SetupId(ItemCategoryId returnValue)
        {
            Setup(i => i.Id)
                .Returns(returnValue);
        }

        public void VerifyDeleteOnce()
        {
            Verify(i => i.Delete(), Times.Once);
        }
    }
}