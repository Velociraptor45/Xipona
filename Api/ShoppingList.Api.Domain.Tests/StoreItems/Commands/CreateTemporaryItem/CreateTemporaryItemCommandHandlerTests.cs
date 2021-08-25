using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateTemporaryItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.StoreItems.Ports;
using ShoppingList.Api.Domain.TestKit.StoreItems.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.StoreItems.Commands.CreateTemporaryItem
{
    public class CreateTemporaryItemCommandHandlerTests
    {
        private readonly LocalFixture _local;

        public CreateTemporaryItemCommandHandlerTests()
        {
            _local = new LocalFixture();
        }

        [Fact]
        public async Task HandleAsync_WithCommandIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var handler = _local.CreateCommandHandler();

            // Act
            Func<Task<bool>> action = async () => await handler.HandleAsync(null, default);

            // Assert
            using (new AssertionScope())
            {
                await action.Should().ThrowAsync<ArgumentNullException>();
            }
        }

        #region WithValidData

        [Fact]
        public async Task HandleAsync_WithValidData_ShouldReturnTrue()
        {
            // Arrange
            var handler = _local.CreateCommandHandler();
            _local.SetupWithValidData();

            // Act
            var result = await handler.HandleAsync(_local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
            }
        }

        [Fact]
        public async Task HandleAsync_WithValidData_ShouldValidateAvailabilities()
        {
            // Arrange
            var handler = _local.CreateCommandHandler();
            _local.SetupWithValidData();

            // Act
            await handler.HandleAsync(_local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                _local.VerifyValidatingAvailabilities();
            }
        }

        [Fact]
        public async Task HandleAsync_WithValidData_ShouldStoreItem()
        {
            // Arrange
            var handler = _local.CreateCommandHandler();
            _local.SetupWithValidData();

            // Act
            await handler.HandleAsync(_local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                _local.VerifyStoringItem();
            }
        }

        #endregion WithValidData

        private sealed class LocalFixture
        {
            public Fixture Fixture { get; }
            public CommonFixture CommonFixture { get; } = new CommonFixture();
            public ItemRepositoryMock ItemRepositoryMock { get; }
            public StoreItemFactoryMock StoreItemFactoryMock { get; }
            public AvailabilityValidationServiceMock AvailabilityValidationServiceMock { get; }

            public CreateTemporaryItemCommand Command { get; private set; }
            public IStoreItem StoreItem { get; private set; }
            public IStoreItemAvailability Availability { get; private set; }

            public LocalFixture()
            {
                Fixture = CommonFixture.GetNewFixture();

                ItemRepositoryMock = new ItemRepositoryMock(Fixture);
                StoreItemFactoryMock = new StoreItemFactoryMock(Fixture);
                AvailabilityValidationServiceMock = new AvailabilityValidationServiceMock(Fixture);
            }

            public void SetupCommand()
            {
                Fixture.ConstructorArgumentFor<TemporaryItemCreation, IStoreItemAvailability>("availability", Availability);
                Command = Fixture.Create<CreateTemporaryItemCommand>();
            }

            public CreateTemporaryItemCommandHandler CreateCommandHandler()
            {
                return Fixture.Create<CreateTemporaryItemCommandHandler>();
            }

            public void SetupStoreItem()
            {
                StoreItem = StoreItemMother.Initial().Create();
            }

            public void SetupRandomAvailability()
            {
                Availability = CommonFixture.ChooseRandom(StoreItem.Availabilities);
            }

            #region Mock Setup

            public void SetupCreatingStoreItem()
            {
                StoreItemFactoryMock.SetupCreate(Command.TemporaryItemCreation, StoreItem);
            }

            #endregion Mock Setup

            #region Verify

            public void VerifyValidatingAvailabilities()
            {
                AvailabilityValidationServiceMock.VerifyValidateOnce(
                    Command.TemporaryItemCreation.Availability.ToMonoList());
            }

            public void VerifyStoringItem()
            {
                ItemRepositoryMock.VerifyStoreAsyncOnce(StoreItem);
            }

            #endregion Verify

            #region Aggregates

            public void SetupWithValidData()
            {
                SetupStoreItem();
                SetupRandomAvailability();
                SetupCommand();
                SetupCreatingStoreItem();
            }

            #endregion Aggregates
        }
    }
}