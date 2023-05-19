using ProjectHermes.ShoppingList.Api.Domain.Items.DomainEvents;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.EventHandlers;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Services.Modifications;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Recipes.EventHandlers;

public class ItemUpdatedDomainEventHandlerTests
{
    private readonly ItemUpdatedDomainEventHandlerFixture _fixture = new();

    [Fact]
    public async Task HandleAsync_WithValidDomainEvent_ShouldModifyIngredients()
    {
        // Arrange
        _fixture.SetupDomainEvent();
        _fixture.SetupModifyingIngredientsAfterItemUpdate();
        var sut = _fixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_fixture.DomainEvent);

        // Act
        await sut.HandleAsync(_fixture.DomainEvent, default);

        // Assert
        _fixture.VerifyModifyingIngredientsAfterItemUpdate();
    }

    private sealed class ItemUpdatedDomainEventHandlerFixture
    {
        private RecipeModificationServiceMock _recipeModificationServiceMock = new(MockBehavior.Strict);
        public ItemUpdatedDomainEvent? DomainEvent { get; private set; }

        public ItemUpdatedDomainEventHandler CreateSut()
        {
            return new ItemUpdatedDomainEventHandler(_ => _recipeModificationServiceMock.Object);
        }

        public void SetupDomainEvent()
        {
            DomainEvent = new DomainTestBuilder<ItemUpdatedDomainEvent>().Create();
        }

        public void SetupModifyingIngredientsAfterItemUpdate()
        {
            TestPropertyNotSetException.ThrowIfNull(DomainEvent);

            _recipeModificationServiceMock.SetupModifyIngredientsAfterItemUpdateAsync(
                DomainEvent.OldItemId, DomainEvent.NewItem);
        }

        public void VerifyModifyingIngredientsAfterItemUpdate()
        {
            TestPropertyNotSetException.ThrowIfNull(DomainEvent);
            _recipeModificationServiceMock.VerifyModifyIngredientsAfterItemUpdateAsync(
                DomainEvent!.OldItemId, DomainEvent.NewItem, Times.Once);
        }
    }
}