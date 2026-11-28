using Moq;
using Xipona.Frontend.Redux.ItemCategories.States;
using Xipona.Frontend.Redux.TestKit;
using Xipona.Frontend.Redux.TestKit.Common;
using Xipona.Frontend.Redux.TestKit.Shared.Services;

namespace Xipona.Frontend.Redux.Tests.ItemCategories.Effects;

public class ItemCategoryEffectsFixtureBase : EffectsFixtureBase
{
    protected readonly ItemCategoryStateMock ItemCategoryStateMock = new(MockBehavior.Strict);
    protected readonly NavigationManagerMock NavigationManagerMock = new(MockBehavior.Strict);
    protected ItemCategoryState State = new DomainTestBuilder<ItemCategoryState>().Create();

    protected void SetupStateReturningState()
    {
        ItemCategoryStateMock.SetupValue(State);
    }
}