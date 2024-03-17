using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Models.Factories;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.Items.Models.Factories;

public class ItemTypeFactoryMock : Mock<IItemTypeFactory>
{
    public ItemTypeFactoryMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupCreateNew(ItemTypeName name, IEnumerable<ItemAvailability> availabilities, IItemType returnValue)
    {
        Setup(m => m.CreateNew(name, availabilities))
            .Returns(returnValue);
    }
}