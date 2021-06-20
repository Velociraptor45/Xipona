using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateTemporaryItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures;
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
        [Fact]
        public async Task HandleAsync_WithCommandIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var local = new LocalFixture();
            var handler = local.CreateCommandHandler();

            // Act
            Func<Task<bool>> action = async () => await handler.HandleAsync(null, default);

            // Assert
            using (new AssertionScope())
            {
                await action.Should().ThrowAsync<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task HandleAsync_WithValidData_ShouldStoreItem()
        {
            // Arrange
            var local = new LocalFixture();
            var handler = local.CreateCommandHandler();

            IStoreItem storeItem = local.StoreItemFixture.CreateValid();
            IStoreItemAvailability availability = local.CommonFixture.ChooseRandom(storeItem.Availabilities);

            var command = local.CreateCommand(availability);
            local.StoreItemFactoryMock.SetupCreate(command.TemporaryItemCreation, storeItem);

            // Act
            var result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                local.AvailabilityValidationServiceMock.VerifyValidateOnce(
                    command.TemporaryItemCreation.Availability.ToMonoList());
                local.ItemRepositoryMock.VerifyStoreAsyncOnce(storeItem);
            }
        }

        private sealed class LocalFixture
        {
            public Fixture Fixture { get; }
            public CommonFixture CommonFixture { get; } = new CommonFixture();
            public StoreItemFixture StoreItemFixture { get; }
            public ItemRepositoryMock ItemRepositoryMock { get; }
            public StoreItemFactoryMock StoreItemFactoryMock { get; }
            public AvailabilityValidationServiceMock AvailabilityValidationServiceMock { get; }

            public LocalFixture()
            {
                Fixture = CommonFixture.GetNewFixture();

                StoreItemFixture = new StoreItemFixture(new StoreItemAvailabilityFixture(CommonFixture), CommonFixture);

                ItemRepositoryMock = new ItemRepositoryMock(Fixture);
                StoreItemFactoryMock = new StoreItemFactoryMock(Fixture);
                AvailabilityValidationServiceMock = new AvailabilityValidationServiceMock(Fixture);
            }

            public CreateTemporaryItemCommand CreateCommand(IStoreItemAvailability availability)
            {
                var fixture = CommonFixture.GetNewFixture();

                fixture.ConstructorArgumentFor<TemporaryItemCreation, IStoreItemAvailability>("availability", availability);

                return fixture.Create<CreateTemporaryItemCommand>();
            }

            public CreateTemporaryItemCommandHandler CreateCommandHandler()
            {
                return Fixture.Create<CreateTemporaryItemCommandHandler>();
            }
        }
    }
}