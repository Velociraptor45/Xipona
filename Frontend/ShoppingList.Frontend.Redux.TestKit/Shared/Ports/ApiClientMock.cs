using Moq;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Shared.Ports;

public class ApiClientMock : Mock<IApiClient>
{
    public ApiClientMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupGetAllActiveStoresForShoppingListAsync(IEnumerable<ShoppingListStore> returnValue)
    {
        Setup(m => m.GetAllActiveStoresForShoppingListAsync()).ReturnsAsync(returnValue);
    }
}