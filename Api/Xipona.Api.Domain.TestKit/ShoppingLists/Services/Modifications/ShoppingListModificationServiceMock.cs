using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Modifications;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.ShoppingLists.Services.Modifications;

public class ShoppingListModificationServiceMock : Mock<IShoppingListModificationService>
{
    public ShoppingListModificationServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupRemoveSectionAsync(SectionId sectionId)
    {
        Setup(m => m.RemoveSectionAsync(sectionId)).Returns(Task.CompletedTask);
    }

    public void SetupAddTemporaryItemAsync(ShoppingListId shoppingListId, ItemName itemName, QuantityType quantityType,
        QuantityInBasket quantity, Price price, SectionId sectionId, TemporaryItemId temporaryItemId)
    {
        Setup(m =>
                m.AddTemporaryItemAsync(shoppingListId, itemName, quantityType, quantity, price, sectionId, temporaryItemId))
            .Returns(Task.CompletedTask);
    }

    public void VerifyAddTemporaryItemAsync(ShoppingListId shoppingListId, ItemName itemName, QuantityType quantityType,
        QuantityInBasket quantity, Price price, SectionId sectionId, TemporaryItemId temporaryItemId, Func<Times> times)
    {
        Verify(m =>
                m.AddTemporaryItemAsync(shoppingListId, itemName, quantityType, quantity, price, sectionId, temporaryItemId),
            times);
    }
}