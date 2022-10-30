using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Updates;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models;

public class ItemTypeMock : Mock<IItemType>
{
    public ItemTypeMock(IItemType itemType, MockBehavior behavior) : base(behavior)
    {
        SetupId(itemType.Id);
        SetupName(itemType.Name);
        SetupAvailabilities(itemType.Availabilities);
        SetupPredecessorId(itemType.PredecessorId);
    }

    public void SetupId(ItemTypeId id)
    {
        Setup(m => m.Id).Returns(id);
    }

    public void SetupName(ItemTypeName name)
    {
        Setup(m => m.Name).Returns(name);
    }

    public void SetupAvailabilities(IEnumerable<IItemAvailability> availabilities)
    {
        Setup(m => m.Availabilities).Returns(availabilities.ToList());
    }

    public void SetupPredecessorId(ItemTypeId? predecessorId)
    {
        Setup(m => m.PredecessorId).Returns(predecessorId);
    }

    public void SetupIsAvailableAtStore(StoreId storeId, bool returnValue)
    {
        Setup(m => m.IsAvailableAt(storeId)).Returns(returnValue);
    }

    public void SetupUpdate(StoreId storeId, Price price, IItemType returnValue)
    {
        Setup(m => m.Update(storeId, price)).Returns(returnValue);
    }

    public void SetupUpdate(IItemType returnValue)
    {
        Setup(m => m.Update()).Returns(returnValue);
    }

    public void SetupUpdateAsync(ItemTypeUpdate update, IValidator validator, IItemType returnValue)
    {
        Setup(m => m.UpdateAsync(update, validator)).ReturnsAsync(returnValue);
    }

    public void VerifyUpdate(StoreId storeId, Price price, Func<Times> times)
    {
        Verify(m => m.Update(storeId, price), times);
    }

    public void VerifyUpdate(Func<Times> times)
    {
        Verify(m => m.Update(), times);
    }
}