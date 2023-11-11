using Blazored.LocalStorage;
using Moq;
using Moq.Contrib.InOrder.Extensions;

namespace ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;

public class LocalStorageServiceMock : Mock<ILocalStorageService>
{
    public LocalStorageServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupSetItemAsStringAsyncForBase64(string key)
    {
        this.SetupInOrder(x => x.SetItemAsStringAsync(key, It.Is<string>(s => s.StartsWith("ey")), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.CompletedTask);
    }

    public void SetupContainKeyAsync(string key, bool result)
    {
        this.SetupInOrder(x => x.ContainKeyAsync(key, It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);
    }

    public void SetupGetItemAsStringAsync(string key, string result)
    {
        this.SetupInOrder(x => x.GetItemAsStringAsync(key, It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);
    }
}