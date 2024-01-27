using AntDesign;
using Moq;
using Moq.Contrib.InOrder;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.WebApp.Services.Notification;
using ShoppingList.Frontend.WebApp.TestKit.Services.Notification;

namespace ShoppingList.Frontend.WebApp.Tests.Services;

public class ShoppingListNotificationServiceTests
{
    public class NotifyAsync
    {
        private readonly NotifyAsyncFixture _fixture = new();

        [Fact]
        public async Task NotifyAsync_WithMessage_CallsNotificationService()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupOpeningNotification();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.NotifyAsync(_fixture.Title, _fixture.Message);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class NotifyAsyncFixture : ShoppingListNotificationServiceFixture
        {
            public string Title { get; } = new DomainTestBuilder<string>().Create();
            public string Message { get; } = new DomainTestBuilder<string>().Create();

            public void SetupOpeningNotification()
            {
                NotificationServiceMock.SetupOpen(new NotificationConfig
                {
                    Message = Title,
                    Description = Message
                });
            }
        }
    }

    public class NotifySuccessAsync
    {
        private readonly NotifySuccessAsyncFixture _fixture = new();

        [Fact]
        public async Task NotifySuccessAsync_WithMessage_CallsNotificationService()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupOpeningNotification();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.NotifySuccessAsync(_fixture.Title, _fixture.Message);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class NotifySuccessAsyncFixture : ShoppingListNotificationServiceFixture
        {
            public string Title { get; } = new DomainTestBuilder<string>().Create();
            public string Message { get; } = new DomainTestBuilder<string>().Create();

            public void SetupOpeningNotification()
            {
                NotificationServiceMock.SetupOpen(new NotificationConfig
                {
                    Message = Title,
                    Description = Message,
                    NotificationType = NotificationType.Success
                });
            }
        }
    }

    public class NotifySuccessAsync_Duration
    {
        private readonly NotifySuccessAsyncFixture _fixture = new();

        [Fact]
        public async Task NotifySuccessAsync_WithDuration_CallsNotificationService()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupOpeningNotification();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.NotifySuccessAsync(_fixture.Message, _fixture.Duration);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task NotifySuccessAsync_WithoutDuration_CallsNotificationService()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupOpeningNotificationWithDefaultDuration();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.NotifySuccessAsync(_fixture.Message);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class NotifySuccessAsyncFixture : ShoppingListNotificationServiceFixture
        {
            public string Message { get; } = new DomainTestBuilder<string>().Create();
            public double Duration { get; } = new DomainTestBuilder<double>().Create();

            public void SetupOpeningNotification()
            {
                NotificationServiceMock.SetupOpen(new NotificationConfig
                {
                    Description = Message,
                    NotificationType = NotificationType.Success,
                    Duration = Duration
                });
            }

            public void SetupOpeningNotificationWithDefaultDuration()
            {
                NotificationServiceMock.SetupOpen(new NotificationConfig
                {
                    Description = Message,
                    NotificationType = NotificationType.Success,
                    Duration = 2
                });
            }
        }
    }

    public class NotifyWarningAsync
    {
        private readonly NotifyWarningAsyncFixture _fixture = new();

        [Fact]
        public async Task NotifyWarningAsync_WithMessage_CallsNotificationService()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupOpeningNotification();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.NotifyWarningAsync(_fixture.Title, _fixture.Message);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class NotifyWarningAsyncFixture : ShoppingListNotificationServiceFixture
        {
            public string Title { get; } = new DomainTestBuilder<string>().Create();
            public string Message { get; } = new DomainTestBuilder<string>().Create();

            public void SetupOpeningNotification()
            {
                NotificationServiceMock.SetupOpen(new NotificationConfig
                {
                    Message = Title,
                    Description = Message,
                    NotificationType = NotificationType.Warning
                });
            }
        }
    }

    public class NotifyErrorAsync
    {
        private readonly NotifyErrorAsyncFixture _fixture = new();

        [Fact]
        public async Task NotifyErrorAsync_WithMessage_CallsNotificationService()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupOpeningNotification();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.NotifyErrorAsync(_fixture.Title, _fixture.Message);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class NotifyErrorAsyncFixture : ShoppingListNotificationServiceFixture
        {
            public string Title { get; } = new DomainTestBuilder<string>().Create();
            public string Message { get; } = new DomainTestBuilder<string>().Create();

            public void SetupOpeningNotification()
            {
                NotificationServiceMock.SetupOpen(new NotificationConfig
                {
                    Message = Title,
                    Description = Message,
                    NotificationType = NotificationType.Error
                });
            }
        }
    }

    private abstract class ShoppingListNotificationServiceFixture
    {
        protected readonly NotificationServiceMock NotificationServiceMock = new(MockBehavior.Strict);

        public ShoppingListNotificationService CreateSut()
        {
            return new(NotificationServiceMock.Object);
        }
    }
}