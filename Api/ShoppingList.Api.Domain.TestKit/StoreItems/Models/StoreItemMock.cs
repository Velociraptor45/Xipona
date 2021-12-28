using Moq;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Models
{
    public class StoreItemMock : Mock<IStoreItem>
    {
        public StoreItemMock(IStoreItem storeItem)
        {
            SetupId(storeItem.Id);
            SetupIsTemporary(storeItem.IsTemporary);
            SetupIsDeleted(storeItem.IsDeleted);
            SetupAvailabilities(storeItem.Availabilities);
            SetupItemTypes(storeItem.ItemTypes);
        }

        private void SetupItemTypes(ItemTypes itemTypes)
        {
            Setup(i => i.ItemTypes)
                .Returns(itemTypes);
        }

        public void SetupId(ItemId returnValue)
        {
            Setup(i => i.Id)
            .Returns(returnValue);
        }

        public void SetupIsTemporary(bool returnValue)
        {
            Setup(i => i.IsTemporary)
                .Returns(returnValue);
        }

        public void SetupIsDeleted(bool returnValue)
        {
            Setup(i => i.IsDeleted)
                .Returns(returnValue);
        }

        public void SetupAvailabilities(IEnumerable<IStoreItemAvailability> returnValue)
        {
            Setup(i => i.Availabilities)
                .Returns(returnValue.ToList().AsReadOnly());
        }

        public void VerifyDeleteOnce()
        {
            Verify(i => i.Delete(), Times.Once);
        }

        public void VerifyDeleteNever()
        {
            Verify(i => i.Delete(), Times.Never);
        }

        public void VerifyMakePermanentOnce(PermanentItem permanentItem,
            IEnumerable<IStoreItemAvailability> availabilities)
        {
            Verify(
                i => i.MakePermanent(
                    It.Is<PermanentItem>(pi => pi == permanentItem),
                    It.Is<IEnumerable<IStoreItemAvailability>>(list => list.SequenceEqual(availabilities))),
                Times.Once);
        }
    }
}