using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Conversion.StoreItemReadModels;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Queries;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Services.Conversion.StoreItemReadModels;

public class StoreItemReadModelConversionServiceMock : Mock<IStoreItemReadModelConversionService>
{
    public StoreItemReadModelConversionServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupConvertAsync(IStoreItem item, StoreItemReadModel returnValue)
    {
        Setup(m => m.ConvertAsync(item, It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnValue);
    }
}