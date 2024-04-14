using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Conversion.ItemSearchReadModels;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Searches;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.TestTools.Extensions;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.Items.Services.Conversion.ItemSearchReadModels;

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