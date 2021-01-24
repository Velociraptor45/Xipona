using Moq;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Mocks
{
    public class StoreItemMock : Mock<IStoreItem>
    {
        public StoreItemMock(IStoreItem storeItem)
        {
            SetupId(storeItem.Id);
        }

        public void SetupId(StoreItemId returnValue)
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