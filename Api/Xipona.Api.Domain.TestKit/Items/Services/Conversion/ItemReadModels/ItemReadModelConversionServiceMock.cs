using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Conversion.ItemReadModels;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Queries;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.Items.Services.Conversion.ItemReadModels;

public class ItemReadModelConversionServiceMock : Mock<IItemReadModelConversionService>
{
    public ItemReadModelConversionServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupConvertAsync(IItem item, ItemReadModel returnValue)
    {
        Setup(m => m.ConvertAsync(item))
            .ReturnsAsync(returnValue);
    }
}