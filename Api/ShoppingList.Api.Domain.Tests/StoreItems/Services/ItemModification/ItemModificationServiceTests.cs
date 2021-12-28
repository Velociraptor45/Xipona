using AutoFixture;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemModification;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Ports;
using ShoppingList.Api.Domain.TestKit.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.StoreItems.Ports;
using ShoppingList.Api.Domain.TestKit.StoreItems.Services.Validation;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.StoreItems.Services.ItemModification
{
    public class ItemModificationServiceTests
    {
        private readonly LocalFixture _fixture;

        public ItemModificationServiceTests()
        {
            _fixture = new LocalFixture();
        }

        [Fact]
        public async Task ModifyItemWithTypesAsync_WithValidData_Should()
        {
        }

        private class LocalFixture
        {
            private readonly Fixture _fixture;
            private readonly ItemRepositoryMock _itemRepositoryMock;
            private readonly ValidatorMock _validatorMock;
            private readonly CommonFixture _commonFixture = new CommonFixture();
            private readonly ShoppingListRepositoryMock _shoppingListRepositoryMock;

            public LocalFixture()
            {
                _fixture = _commonFixture.GetNewFixture();
                _itemRepositoryMock = new ItemRepositoryMock(MockBehavior.Strict);
                _validatorMock = new ValidatorMock(MockBehavior.Strict);
                _shoppingListRepositoryMock = new ShoppingListRepositoryMock(MockBehavior.Strict);
            }

            public ItemWithTypesModification Modification { get; private set; }
            public StoreItemMock Item { get; private set; }

            public ItemModificationService CreateSut()
            {
                return new ItemModificationService(
                    _itemRepositoryMock.Object,
                    _ => _validatorMock.Object,
                    _shoppingListRepositoryMock.Object,
                    default);
            }

            public void SetupModification()
            {
                Modification = _fixture.Create<ItemWithTypesModification>();
            }

            public void SetupFindingItem()
            {
                Item = new StoreItemMock(StoreItemMother.Initial().Create());
                _itemRepositoryMock.SetupFindByAsync(Modification.Id, Item.Object);
            }
        }
    }
}