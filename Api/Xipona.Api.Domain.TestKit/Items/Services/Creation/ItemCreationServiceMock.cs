using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Creations;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Queries;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.Items.Services.Creation;

public class ItemCreationServiceMock : Mock<IItemCreationService>
{
    public ItemCreationServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupCreateItemWithTypesAsync(IItem item, ItemReadModel returnValue)
    {
        Setup(s => s.CreateItemWithTypesAsync(item)).ReturnsAsync(returnValue);
    }

    public void VerifyCreateItemWithTypesAsync(IItem item, Func<Times> times)
    {
        Verify(s => s.CreateItemWithTypesAsync(item), times);
    }

    public void SetupCreateAsync(ItemCreation item, ItemReadModel returnValue)
    {
        Setup(s => s.CreateAsync(item)).ReturnsAsync(returnValue);
    }

    public void VerifyCreateAsync(ItemCreation item, Func<Times> times)
    {
        Verify(s => s.CreateAsync(item), times);
    }
}