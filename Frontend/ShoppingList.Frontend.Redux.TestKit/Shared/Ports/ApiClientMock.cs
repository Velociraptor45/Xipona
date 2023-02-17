using Moq;
using Moq.Sequences;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.Items;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States;
using ProjectHermes.ShoppingList.Frontend.TestTools.Extensions;

namespace ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Shared.Ports;

public class ApiClientMock : Mock<IApiClient>
{
    public ApiClientMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupGetAllActiveStoresForShoppingListAsync(IEnumerable<ShoppingListStore> returnValue)
    {
        Setup(m => m.GetAllActiveStoresForShoppingListAsync())
            .InSequence()
            .ReturnsAsync(returnValue);
    }

    public void SetupUpdateItemPriceAsync(UpdateItemPriceRequest request)
    {
        Setup(m => m.UpdateItemPriceAsync(It.Is<UpdateItemPriceRequest>(r => r.IsRequestEquivalentTo(request))))
            .InSequence()
            .Returns(Task.CompletedTask);
    }

    public void VerifyUpdateItemPriceAsync(UpdateItemPriceRequest request, Func<Times> times)
    {
        Verify(m => m.UpdateItemPriceAsync(It.Is<UpdateItemPriceRequest>(r => r.IsRequestEquivalentTo(request))), times);
    }

    public void SetupFinishListAsync(FinishListRequest request)
    {
        Setup(m => m.FinishListAsync(It.Is<FinishListRequest>(r => r.IsRequestEquivalentTo(request))))
            .InSequence()
            .Returns(Task.CompletedTask);
    }

    public void VerifyFinishListAsync(FinishListRequest request, Func<Times> times)
    {
        Verify(m => m.FinishListAsync(It.Is<FinishListRequest>(r => r.IsRequestEquivalentTo(request))), times);
    }

    public void SetupSearchItemsForShoppingListAsync(string searchInput, Guid storeId,
        IEnumerable<SearchItemForShoppingListResult> returnValue)
    {
        Setup(m => m.SearchItemsForShoppingListAsync(searchInput, storeId, It.IsAny<CancellationToken>()))
            .InSequence()
            .ReturnsAsync(returnValue);
    }
}