using Fluxor;
using Moq;
using Moq.Contrib.InOrder;
using ProjectHermes.Xipona.Api.Contracts.Common;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Actions;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Effects;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Shared.Ports;
using ProjectHermes.Xipona.Frontend.TestTools.Exceptions;
using RestEase;
using System.Text.Json;

namespace ProjectHermes.Xipona.Frontend.Redux.Tests.Shared.Effects;

public class NotificationEffectsTests
{
    public class HandleDisplayErrorNotificationAction
    {
        private readonly HandleDisplayErrorNotificationActionFixture _fixture = new();

        [Fact]
        public async Task HandleDisplayErrorNotificationAction_WithValidData_ShouldCallNotificationService()
        {
            // Arrange
            _fixture.SetupAction();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupNotifyError();
            });
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleDisplayErrorNotificationAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleDisplayErrorNotificationActionFixture : NotificationEffectsFixture
        {
            private readonly string _title = new DomainTestBuilder<string>().Create();
            private readonly string _message = new DomainTestBuilder<string>().Create();
            public DisplayErrorNotificationAction? Action { get; private set; }

            public void SetupNotifyError()
            {
                NotificationServiceMock.SetupNotifyError(_title, _message);
            }

            public void SetupAction()
            {
                Action = new(_title, _message);
            }
        }
    }

    public class HandleDisplayApiExceptionNotificationAction
    {
        private readonly HandleDisplayApiExceptionNotificationActionFixture _fixture = new();

        [Fact]
        public async Task HandleDisplayApiExceptionNotificationAction_WithContractData_ShouldCallNotificationService()
        {
            // Arrange
            _fixture.SetupActionWithSerializedErrorContract();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupNotifyError();
            });
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleDisplayApiExceptionNotificationAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleDisplayApiExceptionNotificationAction_WithoutContractData_ShouldCallNotificationService()
        {
            // Arrange
            _fixture.SetupActionWithoutSerializedErrorContract();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupNotifyErrorWithoutSerializedErrorContract();
            });
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleDisplayApiExceptionNotificationAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleDisplayApiExceptionNotificationActionFixture : NotificationEffectsFixture
        {
            private static readonly string _title = new DomainTestBuilder<string>().Create();
            private static readonly string _message = new DomainTestBuilder<string>().Create();
            public DisplayApiExceptionNotificationAction? Action { get; private set; }

            public void SetupNotifyError()
            {
                NotificationServiceMock.SetupNotifyError(_title, _message);
            }

            public void SetupNotifyErrorWithoutSerializedErrorContract()
            {
                NotificationServiceMock.SetupNotifyErrorAsyncContaining(_title,
                    "failed because response status code does not indicate success");
            }

            public void SetupActionWithoutSerializedErrorContract()
            {
                var apiException = new ApiException(
                    new DomainTestBuilder<HttpRequestMessage>().Create(),
                    new DomainTestBuilder<HttpResponseMessage>().Create(),
                    _message,
                    new ApiExceptionContentDeserializer());
                Action = new(_title, apiException);
            }

            public void SetupActionWithSerializedErrorContract()
            {
                var errorContract = new DomainTestBuilder<ErrorContract>()
                    .FillConstructorWith("message", _message)
                    .Create();
                var errorContentSerialized = JsonSerializer.Serialize(errorContract);
                var apiException = new ApiException(
                    new DomainTestBuilder<HttpRequestMessage>().Create(),
                    new DomainTestBuilder<HttpResponseMessage>().Create(),
                    errorContentSerialized,
                    new ApiExceptionContentDeserializer());
                Action = new(_title, apiException);
            }

            private sealed class ApiExceptionContentDeserializer : IApiExceptionContentDeserializer
            {
                public T Deserialize<T>(string? content)
                {
                    if (content == _message)
                        return (T)(object)null!;

                    return JsonSerializer.Deserialize<T>(content!)!;
                }
            }
        }
    }

    public class HandleDisplayUnhandledErrorAction
    {
        private readonly HandleDisplayUnhandledErrorActionFixture _fixture = new();

        [Fact]
        public async Task HandleDisplayUnhandledErrorAction_WithValidData_ShouldCallNotificationService()
        {
            // Arrange
            _fixture.SetupAction();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupNotifyError();
            });
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleDisplayUnhandledErrorAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleDisplayUnhandledErrorActionFixture : NotificationEffectsFixture
        {
            private readonly string _message = new DomainTestBuilder<string>().Create();
            public DisplayUnhandledErrorAction? Action { get; private set; }

            public void SetupNotifyError()
            {
                NotificationServiceMock.SetupNotifyError("An error occurred", _message);
            }

            public void SetupAction()
            {
                Action = new(_message);
            }
        }
    }

    private abstract class NotificationEffectsFixture
    {
        protected readonly ShoppingListNotificationServiceMock NotificationServiceMock = new(MockBehavior.Strict);
        public Mock<IDispatcher> DispatcherMock { get; } = new(MockBehavior.Strict);

        public NotificationEffects CreateSut()
        {
            return new(NotificationServiceMock.Object);
        }
    }
}