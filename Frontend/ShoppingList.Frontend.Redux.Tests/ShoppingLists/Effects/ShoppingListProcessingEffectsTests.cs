using AutoFixture;
using AutoFixture.Kernel;
using Moq;
using Moq.Contrib.InOrder;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Actions.Processing;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Effects;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Shared.Ports;
using ProjectHermes.ShoppingList.Frontend.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.ShoppingLists.Effects;

public class ShoppingListProcessingEffectsTests
{
    public class HandleApiRequestProcessingErrorOccurredAction
    {
        private readonly HandleApiRequestProcessingErrorOccurredActionFixture _fixture = new();

        public static readonly IEnumerable<object[]> RequestTypes = typeof(IApiRequest)
            .Assembly
            .GetTypes()
            .Where(t => !t.IsAbstract && t.IsAssignableTo(typeof(IApiRequest)))
            .Select(t => new object[] { t })
            .ToList();

        [Theory]
        [MemberData(nameof(RequestTypes))]
        public async Task HandleApiRequestProcessingErrorOccurredAction_WithValidData_ShouldDispatchesWarningNotification(
            Type requestType)
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupAction(requestType);
                _fixture.SetupNotifyWarning();
            });

            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleApiRequestProcessingErrorOccurredAction(_fixture.Action!, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleApiRequestProcessingErrorOccurredActionFixture : ShoppingListProcessingEffectsFixture
        {
            private IApiRequest? _request;
            public ApiRequestProcessingErrorOccurredAction? Action { get; private set; }

            public void SetupNotifyWarning()
            {
                TestPropertyNotSetException.ThrowIfNull(_request);
                NotificationServiceMock.SetupNotifyWarningContains("Request failed", _request.ItemName);
            }

            public void SetupAction(Type requestType)
            {
                var fixture = new Fixture();
                _request = (IApiRequest)fixture.Create(requestType, new SpecimenContext(fixture));
                Action = new ApiRequestProcessingErrorOccurredAction(_request);
            }
        }
    }

    public class HandleApiConnectionDiedAction
    {
        private readonly HandleApiConnectionDiedActionFixture _fixture = new();

        [Fact]
        public async Task HandleApiConnectionDiedAction_WithValidData_ShouldDispatchesWarningNotification()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupNotifyWarning();
            });

            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleApiConnectionDiedAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleApiConnectionDiedActionFixture : ShoppingListProcessingEffectsFixture
        {
            public void SetupNotifyWarning()
            {
                NotificationServiceMock.SetupNotifyWarningContains("Connection interrupted", "Connection to the server was interrupted.");
            }
        }
    }

    public class HandleQueueProcessedAction
    {
        private readonly HandleQueueProcessedActionFixture _fixture = new();

        [Fact]
        public async Task HandleQueueProcessedAction_WithValidData_ShouldDispatchesSuccessNotification()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupNotifySuccess();
                _fixture.SetupDispatchingReloadCurrentShoppingListAction();
            });

            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleQueueProcessedAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleQueueProcessedActionFixture : ShoppingListProcessingEffectsFixture
        {
            public void SetupNotifySuccess()
            {
                NotificationServiceMock.SetupNotifySuccess("Sync completed", "Synchronization with the server completed.");
            }

            public void SetupDispatchingReloadCurrentShoppingListAction()
            {
                SetupDispatchingAction<ReloadCurrentShoppingListAction>();
            }
        }
    }

    public class HandleReloadAfterErrorAction
    {
        private readonly HandleReloadAfterErrorActionFixture _fixture = new();

        [Fact]
        public async Task HandleReloadAfterErrorAction_WithValidData_ShouldDispatchesSelectedStoreChangedAction()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingReloadShoppingListAction();
            });

            // Act
            await ShoppingListProcessingEffects.HandleReloadAfterErrorAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleReloadAfterErrorActionFixture : ShoppingListProcessingEffectsFixture
        {
            public void SetupDispatchingReloadShoppingListAction()
            {
                SetupDispatchingAction<ReloadCurrentShoppingListAction>();
            }
        }
    }

    private abstract class ShoppingListProcessingEffectsFixture : ShoppingListEffectsFixtureBase
    {
        protected readonly ShoppingListNotificationServiceMock NotificationServiceMock = new(MockBehavior.Strict);

        public ShoppingListProcessingEffects CreateSut()
        {
            SetupStateReturningState();
            return new(NotificationServiceMock.Object);
        }
    }
}