using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Items.Ports;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.Domain.TestKit.Items.Ports;

public class ItemTypeReadRepositoryMock : Mock<IItemTypeReadRepository>
{
    public ItemTypeReadRepositoryMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupFindActiveByAsync(string name, StoreId storeId, IEnumerable<ItemId> excludedItemIds,
        IEnumerable<ItemTypeId> excludedItemTypeIds, int? limit, IEnumerable<(ItemId, ItemTypeId)> returnValue)
    {
        Setup(m => m.FindActiveByAsync(name, storeId, excludedItemIds, excludedItemTypeIds, limit))
            .ReturnsAsync(returnValue);
    }
}