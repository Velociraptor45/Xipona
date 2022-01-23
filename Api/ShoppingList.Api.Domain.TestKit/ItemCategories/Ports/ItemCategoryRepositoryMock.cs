using Moq;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.TestKit.ItemCategories.Ports
{
    public class ItemCategoryRepositoryMock : Mock<IItemCategoryRepository>
    {
        public ItemCategoryRepositoryMock(MockBehavior behavior) : base(behavior)
        {
        }

        public void SetupFindByAsync(ItemCategoryId itemCategoryId, IItemCategory returnValue)
        {
            Setup(i => i.FindByAsync(
                    It.Is<ItemCategoryId>(id => id == itemCategoryId),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(returnValue));
        }

        public void SetupFindByAsync(IEnumerable<ItemCategoryId> itemCategoryIds, IEnumerable<IItemCategory> returnValue)
        {
            Setup(i => i.FindByAsync(
                    It.Is<IEnumerable<ItemCategoryId>>(ids => ids.SequenceEqual(itemCategoryIds)),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(returnValue));
        }

        public void VerifyStoreAsyncOnce(IItemCategory itemCategory)
        {
            Verify(i => i.StoreAsync(
                    It.Is<IItemCategory>(cat => cat == itemCategory),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        public void SetupStoreAsync(IItemCategory itemCategory, IItemCategory returnValue)
        {
            Setup(m => m.StoreAsync(itemCategory, It.IsAny<CancellationToken>()))
                .ReturnsAsync(returnValue);
        }
    }
}