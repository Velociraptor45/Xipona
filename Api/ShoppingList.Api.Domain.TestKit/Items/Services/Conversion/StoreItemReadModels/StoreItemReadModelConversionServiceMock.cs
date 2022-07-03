using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Conversion.StoreItemReadModels;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries;

namespace ShoppingList.Api.Domain.TestKit.Items.Services.Conversion.StoreItemReadModels;

public class StoreItemReadModelConversionServiceMock : Mock<IStoreItemReadModelConversionService>
{
    public StoreItemReadModelConversionServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupConvertAsync(IItem item, StoreItemReadModel returnValue)
    {
        Setup(m => m.ConvertAsync(item, It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnValue);
    }
}