using Blazored.LocalStorage;
using Moq;
using Moq.Contrib.InOrder;
using Moq.Contrib.InOrder.Extensions;

namespace Xipona.Frontend.Redux.TestKit.Common;

public class LocalStorageServiceMock : Mock<ILocalStorageService>
{
    public LocalStorageServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupSetItemAsStringAsyncForBase64(string key, IQueueComponent component)
    {
        this.SetupInOrder(x =>
                    x.SetItemAsStringAsync(
                        key,
                        It.Is<string>(s => s.StartsWith("ey")),
                        It.IsAny<CancellationToken>()),
                component)
            .Returns(ValueTask.CompletedTask);
    }

    public void SetupContainKeyAsync(string key, bool result, IQueueComponent component)
    {
        this.SetupInOrder(x => x.ContainKeyAsync(key, It.IsAny<CancellationToken>()), component)
            .ReturnsAsync(result);
    }

    public void SetupGetItemAsStringAsync(string key, string result, IQueueComponent component)
    {
        this.SetupInOrder(x => x.GetItemAsStringAsync(key, It.IsAny<CancellationToken>()), component)
            .ReturnsAsync(result);
    }
}