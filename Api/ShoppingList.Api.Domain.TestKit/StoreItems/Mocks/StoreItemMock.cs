using Moq;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Mocks
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

        public void SetupIsTemporary(bool returnValue)
        {
            Setup(i => i.IsTemporary)
                .Returns(returnValue);
        }

        public void VerifyDeleteOnce()
        {
            Verify(i => i.Delete(), Times.Once);
        }

        public void VerifyMakePermanentOnce(PermanentItem permanentItem, IItemCategory itemCategory,
            IManufacturer manufacturer, IEnumerable<IStoreItemAvailability> availabilities)
        {
            Verify(
                i => i.MakePermanent(
                    It.Is<PermanentItem>(pi => pi == permanentItem),
                    It.Is<IItemCategory>(cat => cat == itemCategory),
                    It.Is<IManufacturer>(man => man == manufacturer),
                    It.Is<IEnumerable<IStoreItemAvailability>>(list => list.SequenceEqual(availabilities))),
                Times.Once);
        }
    }
}