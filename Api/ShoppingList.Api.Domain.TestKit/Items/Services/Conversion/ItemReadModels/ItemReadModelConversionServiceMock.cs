using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Conversion.ItemReadModels;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Services.Conversion.ItemReadModels;

public class ItemReadModelConversionServiceMock : Mock<IItemReadModelConversionService>
{
    public ItemReadModelConversionServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupConvertAsync(IItem item, ItemReadModel returnValue)
    {
        Setup(m => m.ConvertAsync(item, It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnValue);
    }
}