using Moq;
using ProjectHermes.Xipona.Frontend.Redux.ItemCategories.States;
using ProjectHermes.Xipona.Frontend.Redux.TestKit;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Shared.Services;

namespace ProjectHermes.Xipona.Frontend.Redux.Tests.ItemCategories.Effects;

public class ItemCategoryEffectsFixtureBase : EffectsFixtureBase
{
    protected readonly ItemCategoryStateMock ItemCategoryStateMock = new(MockBehavior.Strict);
    protected readonly NavigationManagerMock NavigationManagerMock = new(MockBehavior.Strict);
    protected ItemCategoryState State = new DomainTestBuilder<ItemCategoryState>().Create();

    public void SetupStateReturningState()
    {
        ItemCategoryStateMock.SetupValue(State);
    }
}