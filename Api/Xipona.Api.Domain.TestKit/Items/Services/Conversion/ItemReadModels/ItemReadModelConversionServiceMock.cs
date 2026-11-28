using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Items.Services.Conversion.ItemReadModels;
using Xipona.Api.Domain.Items.Services.Queries;

namespace Xipona.Api.Domain.TestKit.Items.Services.Conversion.ItemReadModels;

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