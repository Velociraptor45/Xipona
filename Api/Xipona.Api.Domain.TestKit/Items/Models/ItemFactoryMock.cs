using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Models.Factories;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Creations;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.Items.Models;

public class ItemFactoryMock : Mock<IItemFactory>
{
    public ItemFactoryMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupCreate(ItemCreation itemCreation, IItem returnValue)
    {
        Setup(i => i.Create(
                It.Is<ItemCreation>(c => c == itemCreation)))
            .Returns(returnValue);
    }
}