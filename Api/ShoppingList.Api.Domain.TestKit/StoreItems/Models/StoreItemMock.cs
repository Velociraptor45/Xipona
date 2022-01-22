using Moq;
using Moq.Language.Flow;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemModification;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Models
{
    public class StoreItemMock : Mock<IStoreItem>
    {
        public StoreItemMock(IStoreItem storeItem, MockBehavior behavior) : base(behavior)
        {
            SetupId(storeItem.Id);
            SetupIsTemporary(storeItem.IsTemporary);
            SetupIsDeleted(storeItem.IsDeleted);
            SetupAvailabilities(storeItem.Availabilities);
            SetupItemTypes(storeItem.ItemTypes);
            SetupHasItemTypes(storeItem.HasItemTypes);
        }

        public bool ModifyWithTypeCalled { get; set; }

        private void SetupItemTypes(ItemTypes itemTypes)
        {
            Setup(i => i.ItemTypes)
                .Returns(itemTypes);
        }

        private void SetupHasItemTypes(bool hasItemTypes)
        {
            Setup(i => i.HasItemTypes)
                .Returns(hasItemTypes);
        }

        public ISetup<IStoreItem, ItemTypes> SetupItemTypes()
        {
            return Setup(i => i.ItemTypes);
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

        public void SetupModifyAsync(ItemWithTypesModification modification, IValidator validator)
        {
            Setup(m => m.ModifyAsync(modification, validator))
                .Callback(() => ModifyWithTypeCalled = true)
                .Returns(Task.CompletedTask);
        }

        public void SetupDelete()
        {
            Setup(i => i.Delete());
        }

        #region Verify

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

        public void VerifyModifyAsync(ItemWithTypesModification modification, IValidator validator, Func<Times> times)
        {
            Verify(m => m.ModifyAsync(modification, validator), times);
        }

        #endregion Verify
    }
}