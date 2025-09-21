using AutoFixture;
using AutoFixture.Kernel;
using Moq;
using Moq.Contrib.InOrder;
using System.Runtime.InteropServices.ObjectiveC;
using Xipona.Frontend.Redux.Shared.Ports.Requests;
using Xipona.Frontend.Redux.ShoppingList.Actions;
using Xipona.Frontend.Redux.ShoppingList.Actions.Processing;
using Xipona.Frontend.Redux.ShoppingList.Effects;
using Xipona.Frontend.Redux.TestKit.Shared.Ports;
using Xipona.Frontend.TestTools.Exceptions;

namespace Xipona.Frontend.Redux.Tests.ShoppingLists.Effects;

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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupAction(requestType);
                _fixture.SetupNotifyWarning(x0);
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

            public void SetupNotifyWarning(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_request);
                NotificationServiceMock.SetupNotifyWarningContains("Request failed", _request.ItemName, component);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupNotifyWarning(x0);
            });

            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleApiConnectionDiedAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleApiConnectionDiedActionFixture : ShoppingListProcessingEffectsFixture
        {
            public void SetupNotifyWarning(IQueueComponent component)
            {
                NotificationServiceMock
                    .SetupNotifyWarningContains("Connection interrupted", "Connection to the server was interrupted.", component);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupNotifySuccess(x0);
                _fixture.SetupDispatchingReloadCurrentShoppingListAction(x0);
            });

            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleQueueProcessedAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleQueueProcessedActionFixture : ShoppingListProcessingEffectsFixture
        {
            public void SetupNotifySuccess(IQueueComponent component)
            {
                NotificationServiceMock
                    .SetupNotifySuccess("Sync completed", "Synchronization with the server completed.", component);
            }

            public void SetupDispatchingReloadCurrentShoppingListAction(IQueueComponent component)
            {
                SetupDispatchingAction<ReloadCurrentShoppingListAction>(component);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingReloadShoppingListAction(x0);
            });

            // Act
            await ShoppingListProcessingEffects.HandleReloadAfterErrorAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleReloadAfterErrorActionFixture : ShoppingListProcessingEffectsFixture
        {
            public void SetupDispatchingReloadShoppingListAction(IQueueComponent component)
            {
                SetupDispatchingAction<ReloadCurrentShoppingListAction>(component);
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