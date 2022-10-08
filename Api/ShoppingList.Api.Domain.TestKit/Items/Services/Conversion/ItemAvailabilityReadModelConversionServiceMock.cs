using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Conversion;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries;
using ProjectHermes.ShoppingList.Api.TestTools.Extensions;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Services.Conversion;

public class ItemAvailabilityReadModelConversionServiceMock : Mock<IItemAvailabilityReadModelConversionService>
{
    public ItemAvailabilityReadModelConversionServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupConvertAsync(IEnumerable<IItem> items,
        IDictionary<(ItemId, ItemTypeId?), IEnumerable<ItemAvailabilityReadModel>> returnValue)
    {
        Setup(m => m.ConvertAsync(It.Is<IEnumerable<IItem>>(it => it.IsEquivalentTo(items))))
            .ReturnsAsync(returnValue);
    }
}