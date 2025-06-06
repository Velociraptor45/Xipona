using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Items.Services.Conversion.ItemSearchReadModels;
using Xipona.Api.Domain.Items.Services.Searches;
using Xipona.Api.Domain.Stores.Models;
using Xipona.Api.TestTools.Extensions;

namespace Xipona.Api.Domain.TestKit.Items.Services.Conversion.ItemSearchReadModels;

public class ItemSearchReadModelConversionServiceMock : Mock<IItemSearchReadModelConversionService>
{
    public ItemSearchReadModelConversionServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupConvertAsync(IEnumerable<IItem> items, IStore store,
        IEnumerable<SearchItemForShoppingResultReadModel> returnValue)
    {
        Setup(m => m.ConvertAsync(
                It.Is<IEnumerable<IItem>>(i => i.IsEquivalentTo(items)),
                store))
            .ReturnsAsync(returnValue);
    }

    public void SetupConvertAsync(IEnumerable<ItemWithMatchingItemTypeIds> mappings, IStore store,
        IEnumerable<SearchItemForShoppingResultReadModel> returnValue)
    {
        Setup(m => m.ConvertAsync(
                It.Is<IEnumerable<ItemWithMatchingItemTypeIds>>(maps => maps.IsEquivalentTo(mappings)),
                store))
            .ReturnsAsync(returnValue);
    }
}