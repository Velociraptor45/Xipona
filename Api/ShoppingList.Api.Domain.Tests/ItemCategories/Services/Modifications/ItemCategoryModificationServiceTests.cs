using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Modifications;
using ShoppingList.Api.Domain.TestKit.Common;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Models;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Ports;
using ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ItemCategories.Services.Modifications;

public class ItemCategoryModificationServiceTests
{
    private readonly ItemCategoryModificationServiceFixture _fixture;

    public ItemCategoryModificationServiceTests()
    {
        _fixture = new ItemCategoryModificationServiceFixture();
    }

    [Fact]
    public async Task ModifyAsync_WithValidData_ShouldCallModel()
    {
        // Arrange
        _fixture.SetupModification();
        _fixture.SetupFindingItemCategory();
        _fixture.SetupModifyingItemCategory();
        _fixture.SetupStoringItemCategory();
        var sut = _fixture.CreateSut();

        // Act
        await sut.ModifyAsync(_fixture.Modification!);

        // Assert
        _fixture.VerifyModifyingItemCategory();
    }

    public class ItemCategoryModificationServiceFixture
    {
        private readonly ItemCategoryRepositoryMock _itemCategoryRepositoryMock = new(MockBehavior.Strict);
        private ItemCategoryMock? _itemCategoryMock;

        public ItemCategoryModification? Modification { get; private set; }

        public ItemCategoryModificationService CreateSut()
        {
            return new ItemCategoryModificationService(_itemCategoryRepositoryMock.Object, default);
        }

        public void SetupModification()
        {
            Modification = new DomainTestBuilder<ItemCategoryModification>().Create();
        }

        public void SetupFindingItemCategory()
        {
            TestPropertyNotSetException.ThrowIfNull(Modification);

            _itemCategoryMock = new ItemCategoryMock(new ItemCategoryBuilder().Create(), MockBehavior.Strict);
            _itemCategoryRepositoryMock.SetupFindByAsync(Modification.Id, _itemCategoryMock.Object);
        }

        public void SetupModifyingItemCategory()
        {
            TestPropertyNotSetException.ThrowIfNull(_itemCategoryMock);

            _itemCategoryMock.SetupModify(Modification);
        }

        public void SetupStoringItemCategory()
        {
            TestPropertyNotSetException.ThrowIfNull(_itemCategoryMock);

            _itemCategoryRepositoryMock.SetupStoreAsync(_itemCategoryMock.Object, _itemCategoryMock.Object);
        }

        public void VerifyModifyingItemCategory()
        {
            TestPropertyNotSetException.ThrowIfNull(_itemCategoryMock);

            _itemCategoryMock.VerifyModify(Modification, Times.Once);
        }
    }
}