using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Models.Fixtures;
using ShoppingList.Api.Core.Extensions;
using ShoppingList.Api.Core.Tests.AutoFixture;
using ShoppingList.Api.Domain.Commands.PutItemInBasket;
using ShoppingList.Api.Domain.Models;
using ShoppingList.Api.Domain.Ports;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

using DomainModels = ShoppingList.Api.Domain.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Commands.PutItemInBasket
{
    public class PutItemInBasketCommandHandlerTests
    {
        private readonly CommonFixture commonFixture;
        private readonly ShoppingListItemFixture shoppingListItemFixture;
        private readonly ShoppingListFixture shoppingListFixture;

        public PutItemInBasketCommandHandlerTests()
        {
            commonFixture = new CommonFixture();
            shoppingListItemFixture = new ShoppingListItemFixture(commonFixture);
            shoppingListFixture = new ShoppingListFixture(shoppingListItemFixture, commonFixture);
        }

        [Fact]
        public async Task HandleAsync_WithCommandIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();
            var handler = fixture.Create<PutItemInBasketCommandHandler>();

            // Act
            Func<Task> function = async () => await handler.HandleAsync(null, default);

            // Assert
            using (new AssertionScope())
            {
                await function.Should().ThrowAsync<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task HandleAsync_WithValidActualIdCommand_ShouldPutItemInBasket()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();
            var itemNotInBasket = shoppingListItemFixture.GetShoppingListItem(isInBasket: false);
            var shoppingList = shoppingListFixture.GetShoppingList(itemCount: 0, itemNotInBasket.ToMonoList());
            var shoppingListRepositoryMock = fixture.Freeze<Mock<IShoppingListRepository>>();

            fixture.ConstructorArgumentFor<PutItemInBasketCommand, ShoppingListItemId>("itemId", itemNotInBasket.Id);
            var command = fixture.Create<PutItemInBasketCommand>();
            var handler = fixture.Create<PutItemInBasketCommandHandler>();

            shoppingListRepositoryMock
                .Setup(instance => instance.FindByAsync(
                    It.Is<ShoppingListId>(id => id == command.ShoppingListId),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(shoppingList));

            // Act
            bool result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                shoppingList.Items.Should().ContainSingle();
                shoppingList.Items.First().IsInBasket.Should().BeTrue();
                shoppingListRepositoryMock.Verify(
                    i => i.StoreAsync(
                        It.Is<DomainModels.ShoppingList>(list => list.Id == shoppingList.Id),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
            }
        }
    }
}