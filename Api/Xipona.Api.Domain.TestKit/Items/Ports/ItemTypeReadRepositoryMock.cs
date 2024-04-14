using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Ports;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.Items.Ports;

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