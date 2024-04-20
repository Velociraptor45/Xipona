using Moq;
using Moq.Contrib.InOrder;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.Persistence;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Effects;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;
using ProjectHermes.Xipona.Frontend.TestTools.Exceptions;
using System.Text;
using System.Text.Json;

namespace ProjectHermes.Xipona.Frontend.Redux.Tests.ShoppingLists.Effects;

public class ShoppingListPersistenceEffectsTests
{
    private readonly ShoppingListPersistenceEffectsFixture _fixture = new();

    [Fact]
    public async Task HandlePutItemInBasketAction_WithValidData_ShouldStoreInLocalStorage()
    {
        // Arrange
        var queue = CallQueue.Create(_ =>
        {
            _fixture.SetupSelectedStore();
            _fixture.SetupStoringInLocalStorage();
        });
        var sut = _fixture.CreateSut();

        // Act
        await sut.HandlePutItemInBasketAction(_fixture.DispatcherMock.Object);

        // Assert
        queue.VerifyOrder();
    }

    [Fact]
    public async Task HandleRemoveItemFromBasketAction_WithValidData_ShouldStoreInLocalStorage()
    {
        // Arrange
        var queue = CallQueue.Create(_ =>
        {
            _fixture.SetupSelectedStore();
            _fixture.SetupStoringInLocalStorage();
        });
        var sut = _fixture.CreateSut();

        // Act
        await sut.HandleRemoveItemFromBasketAction(_fixture.DispatcherMock.Object);

        // Assert
        queue.VerifyOrder();
    }

    [Fact]
    public async Task HandleRemoveItemFromShoppingListAction_WithValidData_ShouldStoreInLocalStorage()
    {
        // Arrange
        var queue = CallQueue.Create(_ =>
        {
            _fixture.SetupSelectedStore();
            _fixture.SetupStoringInLocalStorage();
        });
        var sut = _fixture.CreateSut();

        // Act
        await sut.HandleRemoveItemFromShoppingListAction(_fixture.DispatcherMock.Object);

        // Assert
        queue.VerifyOrder();
    }

    [Fact]
    public async Task HandleChangeItemQuantityAction_WithValidData_ShouldStoreInLocalStorage()
    {
        // Arrange
        var queue = CallQueue.Create(_ =>
        {
            _fixture.SetupSelectedStore();
            _fixture.SetupStoringInLocalStorage();
        });
        var sut = _fixture.CreateSut();

        // Act
        await sut.HandleChangeItemQuantityAction(_fixture.DispatcherMock.Object);

        // Assert
        queue.VerifyOrder();
    }

    [Fact]
    public async Task HandleSaveTemporaryItemFinishedAction_WithValidData_ShouldStoreInLocalStorage()
    {
        // Arrange
        var queue = CallQueue.Create(_ =>
        {
            _fixture.SetupSelectedStore();
            _fixture.SetupStoringInLocalStorage();
        });
        var sut = _fixture.CreateSut();

        // Act
        await sut.HandleSaveTemporaryItemFinishedAction(_fixture.DispatcherMock.Object);

        // Assert
        queue.VerifyOrder();
    }

    [Fact]
    public async Task HandleLoadShoppingListFinishedAction_WithValidData_ShouldStoreInLocalStorage()
    {
        // Arrange
        var queue = CallQueue.Create(_ =>
        {
            _fixture.SetupSelectedStore();
            _fixture.SetupStoringInLocalStorage();
        });
        var sut = _fixture.CreateSut();

        // Act
        await sut.HandleLoadShoppingListFinishedAction(_fixture.DispatcherMock.Object);

        // Assert
        queue.VerifyOrder();
    }

    [Fact]
    public async Task HandleLoadShoppingListFromLocalStorageAction_WithExistingList_ShouldDispatchCorrectActions()
    {
        // Arrange
        var queue = CallQueue.Create(_ =>
        {
            _fixture.SetupSelectedStore();
            _fixture.SetupLoadingFromLocalStorageSuccess();
            _fixture.SetupDispatchingFinishAction();
            _fixture.SetupAction();
        });
        var sut = _fixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

        // Act
        await sut.HandleLoadShoppingListFromLocalStorageAction(_fixture.Action, _fixture.DispatcherMock.Object);

        // Assert
        queue.VerifyOrder();
    }

    [Fact]
    public async Task HandleLoadShoppingListFromLocalStorageAction_WithNoExistingList_ShouldDispatchCorrectActions()
    {
        // Arrange
        var queue = CallQueue.Create(_ =>
        {
            _fixture.SetupSelectedStore();
            _fixture.SetupLoadingFromLocalStorageFailure();
            _fixture.SetupDispatchingErrorNotificationAction();
            _fixture.SetupAction();
        });
        var sut = _fixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

        // Act
        await sut.HandleLoadShoppingListFromLocalStorageAction(_fixture.Action, _fixture.DispatcherMock.Object);

        // Assert
        queue.VerifyOrder();
    }

    private sealed class ShoppingListPersistenceEffectsFixture : ShoppingListEffectsFixtureBase
    {
        private readonly LocalStorageServiceMock _localStorageServiceMock = new(MockBehavior.Strict);
        private Guid? _storeId;
        private ShoppingListModel? _expectedShoppingList;
        public LoadShoppingListFromLocalStorageAction? Action { get; private set; }

        public void SetupSelectedStore()
        {
            _storeId = Guid.NewGuid();
            State = State with { SelectedStoreId = _storeId.Value };
        }

        public void SetupStoringInLocalStorage()
        {
            _localStorageServiceMock.SetupSetItemAsStringAsyncForBase64($"list-{_storeId:D}");
        }

        public void SetupLoadingFromLocalStorageFailure()
        {
            var key = $"list-{_storeId:D}";
            _localStorageServiceMock.SetupContainKeyAsync(key, false);
        }

        public void SetupLoadingFromLocalStorageSuccess()
        {
            var key = $"list-{_storeId:D}";
            _expectedShoppingList = new DomainTestBuilder<ShoppingListModel>().Create();
            var base64 = Convert.ToBase64String(
                Encoding.UTF8.GetBytes(
                    JsonSerializer.Serialize(
                        _expectedShoppingList,
                        new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                            PropertyNameCaseInsensitive = true
                        })));

            _localStorageServiceMock.SetupContainKeyAsync(key, true);
            _localStorageServiceMock.SetupGetItemAsStringAsync(key, base64);
        }

        public void SetupDispatchingFinishAction()
        {
            // There's a bug in the comparison framework, thus we can only check for the action in general. See:
            // https://github.com/ValeraT1982/ObjectsComparer/issues/57
            SetupDispatchingAnyAction<LoadShoppingListFinishedAction>();
        }

        public ShoppingListPersistenceEffects CreateSut()
        {
            SetupStateReturningState();
            return new(ShoppingListStateMock.Object, _localStorageServiceMock.Object);
        }

        public void SetupAction()
        {
            TestPropertyNotSetException.ThrowIfNull(_storeId);
            Action = new LoadShoppingListFromLocalStorageAction(_storeId.Value);
        }
    }
}