using Microsoft.Extensions.Logging;
using Moq.Contrib.InOrder;
using ProjectHermes.Xipona.Frontend.Infrastructure.Connection;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports.Requests;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.Processing;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Shared.Ports;
using ProjectHermes.Xipona.Frontend.TestTools;
using ProjectHermes.Xipona.Frontend.TestTools.Exceptions;
using RestEase;
using Xipona.Frontend.Infrastructure.TestKit.RequestSenders;
using System.Net;
using System.Reflection;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Tests.Connection;

public class CommandQueueTests
{
    private const string _collectionName = "CommandQueue";

    [Collection(_collectionName)]
    public class RetryConnectionAsync
    {
        private readonly RetryConnectionAsyncFixture _fixture = new();

        [Fact]
        public async Task RetryConnectionAsync_WithApiNotAlive_ShouldDoNothing()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupApiNotAlive();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.RetryConnectionAsync();

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task RetryConnectionAsync_WithApiAlive_WithEmptyQueue_ShouldDispatchProcessed()
        {
            // Arrange
            _fixture.SetupEmptyQueue();
            _fixture.FillQueueThroughBackdoor();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupApiAlive();
                _fixture.SetupDispatchingConnectionRecoverAction();
                _fixture.SetupDispatchingProcessedAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.RetryConnectionAsync();

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task RetryConnectionAsync_WithApiAlive_WithItemInQueue_WithApiCallSucceeded_ShouldCallApiAndDispatchProcessed()
        {
            // Arrange
            _fixture.SetupOneQueueItem();
            _fixture.FillQueueThroughBackdoor();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupApiAlive();
                _fixture.SetupSendingRequestSucceeded();
                _fixture.SetupDispatchingConnectionRecoverAction();
                _fixture.SetupDispatchingProcessedAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.RetryConnectionAsync();

            // Assert
            queue.VerifyOrder();
            CommandQueueFixture.VerifyQueueEmpty();
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.UnprocessableEntity)]
        public async Task RetryConnectionAsync_WithApiAlive_WithItemInQueue_WithApiException_WithExpectedStatusCode_ShouldDispatchErrorAction(
            HttpStatusCode statusCode)
        {
            // Arrange
            _fixture.SetupOneQueueItem();
            _fixture.FillQueueThroughBackdoor();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupApiAlive();
                _fixture.SetupSendingRequestFailedWithApiException(statusCode);
                _fixture.SetupDispatchingErrorOccurredAction();
                _fixture.SetupDispatchingLogAction();
                _fixture.SetupDispatchingRequestReloadAction();
                _fixture.SetupDispatchingConnectionRecoverAction();
                _fixture.SetupDispatchingProcessedAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.RetryConnectionAsync();

            // Assert
            queue.VerifyOrder();
            CommandQueueFixture.VerifyQueueEmpty();
        }

        [Theory]
        [InlineData(HttpStatusCode.Conflict)]
        [InlineData(HttpStatusCode.Forbidden)]
        public async Task RetryConnectionAsync_WithApiAlive_WithItemInQueue_WithApiException_WithUnexpectedStatusCode_ShouldDispatchConnectionDiedAction(
            HttpStatusCode statusCode)
        {
            // Arrange
            _fixture.SetupOneQueueItem();
            _fixture.FillQueueThroughBackdoor();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupApiAlive();
                _fixture.SetupSendingRequestFailedWithApiException(statusCode);
                _fixture.SetupDispatchingConnectionDiedAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.RetryConnectionAsync();

            // Assert
            queue.VerifyOrder();
            CommandQueueFixture.VerifyQueueNotEmpty();
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.UnprocessableEntity)]
        public async Task RetryConnectionAsync_WithApiAlive_WithItemInQueue_WithHttpRequestException_WithExpectedStatusCode_ShouldDispatchErrorAction(
            HttpStatusCode statusCode)
        {
            // Arrange
            _fixture.SetupOneQueueItem();
            _fixture.FillQueueThroughBackdoor();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupApiAlive();
                _fixture.SetupSendingRequestFailedWithHttpRequestException(statusCode);
                _fixture.SetupDispatchingErrorOccurredAction();
                _fixture.SetupDispatchingLogAction();
                _fixture.SetupDispatchingRequestReloadAction();
                _fixture.SetupDispatchingConnectionRecoverAction();
                _fixture.SetupDispatchingProcessedAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.RetryConnectionAsync();

            // Assert
            queue.VerifyOrder();
            CommandQueueFixture.VerifyQueueEmpty();
        }

        [Theory]
        [InlineData(HttpStatusCode.Conflict)]
        [InlineData(HttpStatusCode.Forbidden)]
        public async Task RetryConnectionAsync_WithApiAlive_WithItemInQueue_WithHttpRequestException_WithUnexpectedStatusCode_ShouldConnectionDiedAction(
            HttpStatusCode statusCode)
        {
            // Arrange
            _fixture.SetupOneQueueItem();
            _fixture.FillQueueThroughBackdoor();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupApiAlive();
                _fixture.SetupSendingRequestFailedWithHttpRequestException(statusCode);
                _fixture.SetupDispatchingConnectionDiedAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.RetryConnectionAsync();

            // Assert
            queue.VerifyOrder();
            CommandQueueFixture.VerifyQueueNotEmpty();
        }

        [Fact]
        public async Task RetryConnectionAsync_WithApiAlive_WithItemInQueue_WithOtherException_ShouldClearQueue()
        {
            // Arrange
            _fixture.SetupFiveQueueItems();
            _fixture.FillQueueThroughBackdoor();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupApiAlive();
                _fixture.SetupSendingRequestFailedWithDefaultException();
                _fixture.SetupDispatchingLogAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.RetryConnectionAsync();

            // Assert
            queue.VerifyOrder();
            CommandQueueFixture.VerifyQueueEmpty();
        }

        private sealed class RetryConnectionAsyncFixture : CommandQueueFixture
        {
            public void SetupApiAlive()
            {
                ApiClientMock.SetupIsAliveAsync();
            }

            public void SetupApiNotAlive()
            {
                ApiClientMock.SetupIsAliveAsyncThrowing(new TestBuilder<HttpRequestException>().Create());
            }

            public void SetupDispatchingProcessedAction()
            {
                DispatcherMock.SetupDispatch(new QueueProcessedAction());
            }

            public void SetupDispatchingConnectionRecoverAction()
            {
                DispatcherMock.SetupDispatch(new ApiConnectionRecoveredAction());
            }

            public void SetupDispatchingErrorOccurredAction()
            {
                TestPropertyNotSetException.ThrowIfNull(Queue);
                DispatcherMock.SetupDispatch(new ApiRequestProcessingErrorOccurredAction(Queue.First()));
            }

            public void SetupSendingRequestSucceeded()
            {
                TestPropertyNotSetException.ThrowIfNull(Queue);
                RequestSenderStrategyMock.SetupSendAsync(Queue.First());
            }

            public void SetupSendingRequestFailedWithApiException(HttpStatusCode statusCode)
            {
                TestPropertyNotSetException.ThrowIfNull(Queue);

                var response = new HttpResponseMessage(statusCode);
                var exception = new TestBuilder<ApiException>().FillConstructorWith("response", response).Create();
                RequestSenderStrategyMock.SetupSendAsyncThrowing(Queue.First(), exception);
            }

            public void SetupSendingRequestFailedWithHttpRequestException(HttpStatusCode statusCode)
            {
                TestPropertyNotSetException.ThrowIfNull(Queue);

                var exception = new HttpRequestException(new TestBuilder<string>().Create(), null, statusCode);
                RequestSenderStrategyMock.SetupSendAsyncThrowing(Queue.First(), exception);
            }
        }
    }

    [Collection(_collectionName)]
    public class Enqueue
    {
        private readonly EnqueueFixture _fixture = new();

        [Fact]
        public async Task Enqueue_WithEmptyQueue_WithConnectionAlive_WithRequestSucceeded_ShouldSendRequestAndEmptyQueue()
        {
            // Arrange
            _fixture.SetupEmptyQueue();
            _fixture.FillQueueThroughBackdoor();
            _fixture.SetupRequest();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupSendingRequestSucceeded();
            });
            var sut = _fixture.CreateSut();
            EnqueueFixture.SetupConnectionAlive(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.Request);

            // Act
            await sut.Enqueue(_fixture.Request);

            // Assert
            queue.VerifyOrder();
            CommandQueueFixture.VerifyQueueEmpty();
        }

        [Fact]
        public async Task Enqueue_WithEmptyQueue_WithConnectionNotAlive_ShouldNotSendRequest()
        {
            // Arrange
            _fixture.SetupEmptyQueue();
            _fixture.FillQueueThroughBackdoor();
            _fixture.SetupRequest();
            var queue = CallQueue.Create(_ => { });
            var sut = _fixture.CreateSut();
            EnqueueFixture.SetupConnectionNotAlive(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.Request);

            // Act
            await sut.Enqueue(_fixture.Request);

            // Assert
            queue.VerifyOrder();
            CommandQueueFixture.VerifyQueueNotEmpty();
        }

        [Fact]
        public async Task Enqueue_WithEmptyQueue_WithConnectionAlive_WithQueueAlreadyFilled_ShouldNotSendRequest()
        {
            // Arrange
            _fixture.SetupOneQueueItem();
            _fixture.FillQueueThroughBackdoor();
            _fixture.SetupRequest();
            var queue = CallQueue.Create(_ => { });
            var sut = _fixture.CreateSut();
            EnqueueFixture.SetupConnectionAlive(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.Request);

            // Act
            await sut.Enqueue(_fixture.Request);

            // Assert
            queue.VerifyOrder();
            CommandQueueFixture.VerifyQueueNotEmpty(2);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.UnprocessableEntity)]
        public async Task Enqueue_WithEmptyQueue_WithConnectionAlive_WithApiException_ShouldDispatchErrorAction(
            HttpStatusCode statusCode)
        {
            // Arrange
            _fixture.SetupEmptyQueue();
            _fixture.FillQueueThroughBackdoor();
            _fixture.SetupRequest();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupSendingRequestFailedWithApiException(statusCode);
                _fixture.SetupDispatchingErrorOccurredAction();
                _fixture.SetupDispatchingLogAction();
                _fixture.SetupDispatchingRequestReloadAction();
            });
            var sut = _fixture.CreateSut();
            EnqueueFixture.SetupConnectionAlive(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.Request);

            // Act
            await sut.Enqueue(_fixture.Request);

            // Assert
            queue.VerifyOrder();
            CommandQueueFixture.VerifyQueueEmpty();
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.UnprocessableEntity)]
        public async Task Enqueue_WithEmptyQueue_WithConnectionAlive_WithHttpRequestException_ShouldDispatchErrorAction(
            HttpStatusCode statusCode)
        {
            // Arrange
            _fixture.SetupEmptyQueue();
            _fixture.FillQueueThroughBackdoor();
            _fixture.SetupRequest();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupSendingRequestFailedWithHttpRequestException(statusCode);
                _fixture.SetupDispatchingErrorOccurredAction();
                _fixture.SetupDispatchingLogAction();
                _fixture.SetupDispatchingRequestReloadAction();
            });
            var sut = _fixture.CreateSut();
            EnqueueFixture.SetupConnectionAlive(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.Request);

            // Act
            await sut.Enqueue(_fixture.Request);

            // Assert
            queue.VerifyOrder();
            CommandQueueFixture.VerifyQueueEmpty();
        }

        private sealed class EnqueueFixture : CommandQueueFixture
        {
            public PutItemInBasketRequest? Request { get; private set; }

            public void SetupRequest()
            {
                Request = new TestBuilder<PutItemInBasketRequest>().Create();
            }

            public void SetupSendingRequestSucceeded()
            {
                TestPropertyNotSetException.ThrowIfNull(Request);
                RequestSenderStrategyMock.SetupSendAsync(Request);
            }

            public void SetupDispatchingErrorOccurredAction()
            {
                TestPropertyNotSetException.ThrowIfNull(Request);
                DispatcherMock.SetupDispatch(new ApiRequestProcessingErrorOccurredAction(Request));
            }

            public void SetupSendingRequestFailedWithApiException(HttpStatusCode statusCode)
            {
                TestPropertyNotSetException.ThrowIfNull(Request);

                var response = new HttpResponseMessage(statusCode);
                var exception = new TestBuilder<ApiException>().FillConstructorWith("response", response).Create();
                RequestSenderStrategyMock.SetupSendAsyncThrowing(Request, exception);
            }

            public void SetupSendingRequestFailedWithHttpRequestException(HttpStatusCode statusCode)
            {
                TestPropertyNotSetException.ThrowIfNull(Request);

                var exception = new HttpRequestException(new TestBuilder<string>().Create(), null, statusCode);
                RequestSenderStrategyMock.SetupSendAsyncThrowing(Request, exception);
            }

            public static void SetupConnectionAlive(CommandQueue sut)
            {
                SetConnection(sut, true);
            }

            public static void SetupConnectionNotAlive(CommandQueue sut)
            {
                SetConnection(sut, false);
            }

            private static void SetConnection(CommandQueue sut, bool isAlive)
            {
                var connectionField = typeof(CommandQueue)
                    .GetField("_connectionAlive", BindingFlags.NonPublic | BindingFlags.Instance);

                connectionField!.SetValue(sut, isAlive);
            }
        }
    }

    private abstract class CommandQueueFixture
    {
        protected readonly ApiClientMock ApiClientMock = new(MockBehavior.Strict);
        protected readonly RequestSenderStrategyMock RequestSenderStrategyMock = new(MockBehavior.Strict);
        protected readonly DispatcherMock DispatcherMock = new(MockBehavior.Strict);
        private readonly Mock<ILogger<CommandQueue>> _loggerMock = new(MockBehavior.Loose);

        private readonly CommandQueueConfig _config = new()
        {
            ConnectionRetryInterval = TimeSpan.Zero
        };

        protected List<IApiRequest>? Queue;

        public CommandQueue CreateSut()
        {
            return new CommandQueue(ApiClientMock.Object, RequestSenderStrategyMock.Object,
                DispatcherMock.Object, _config, _loggerMock.Object);
        }

        public void SetupEmptyQueue()
        {
            Queue = new List<IApiRequest>(0);
        }

        public void SetupOneQueueItem()
        {
            SetupQueueItems(1);
        }

        public void SetupFiveQueueItems()
        {
            SetupQueueItems(5);
        }

        private void SetupQueueItems(int count)
        {
            Queue = new DomainTestBuilder<PutItemInBasketRequest>().CreateMany(count).ToList<IApiRequest>();
        }

        public void SetupDispatchingRequestReloadAction()
        {
            DispatcherMock.SetupDispatch(new ReloadCurrentShoppingListAction());
        }

        public void SetupDispatchingConnectionDiedAction()
        {
            DispatcherMock.SetupDispatch(new ApiConnectionDiedAction());
        }

        public void SetupDispatchingLogAction()
        {
            DispatcherMock.SetupDispatchAny<LogAction>();
        }

        public void SetupSendingRequestFailedWithDefaultException()
        {
            TestPropertyNotSetException.ThrowIfNull(Queue);

            var exception = new TestBuilder<Exception>().Create();
            RequestSenderStrategyMock.SetupSendAsyncThrowing(Queue.First(), exception);
        }

        public void FillQueueThroughBackdoor()
        {
            TestPropertyNotSetException.ThrowIfNull(Queue);

            var internalQueue = GetInternalQueue();
            internalQueue.Clear();
            internalQueue.AddRange(Queue);
        }

        public static void VerifyQueueEmpty()
        {
            var internalQueue = GetInternalQueue();
            internalQueue.Should().BeEmpty();
        }

        public static void VerifyQueueNotEmpty(int expectedCount = 1)
        {
            var internalQueue = GetInternalQueue();
            internalQueue.Should().HaveCount(expectedCount);
        }

        private static List<IApiRequest> GetInternalQueue()
        {
            var queueField = typeof(CommandQueue).GetField("_queue", BindingFlags.NonPublic | BindingFlags.Static);

            return (List<IApiRequest>)queueField!.GetValue(null)!;
        }
    }
}