using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace ProjectHermes.Xipona.Api.Core.TestKit;

public class MemoryCacheMock : Mock<IMemoryCache>
{
    public MemoryCacheMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupTryGetValue<T>(object key, T? outValue, bool returnValue)
    {
        Setup(m => m.TryGetValue(key, out It.Ref<object?>.IsAny))
            .Returns((object _, ref object? type) =>
            {
                type = outValue;
                return returnValue;
            });
    }
}
