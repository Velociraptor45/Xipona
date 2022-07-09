using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Modifications;
using ShoppingList.Api.Domain.TestKit.Common;
using ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
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
    public async Task ModifyAsync_WithValidData_ShouldCallItemCategory()
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

    [Fact]
    public async Task ModifyAsync_WithValidData_ShouldItemCategory()
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
        _fixture.VerifyStoringItemCategory();
    }

    [Fact]
    public async Task ModifyAsync_WithInvalidItemCategoryId_ShouldThrowDomainException()
    {
        // Arrange
        _fixture.SetupModification();
        _fixture.SetupNotFindingItemCategory();
        var sut = _fixture.CreateSut();

        // Act
        var func = async () => await sut.ModifyAsync(_fixture.Modification!);

        // Assert
        await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.ItemCategoryNotFound);
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

        public void SetupNotFindingItemCategory()
        {
            TestPropertyNotSetException.ThrowIfNull(Modification);

            _itemCategoryRepositoryMock.SetupFindByAsync(Modification.Id, null);
        }

        public void SetupModifyingItemCategory()
        {
            TestPropertyNotSetException.ThrowIfNull(_itemCategoryMock);
            TestPropertyNotSetException.ThrowIfNull(Modification);

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
            TestPropertyNotSetException.ThrowIfNull(Modification);

            _itemCategoryMock.VerifyModify(Modification, Times.Once);
        }

        public void VerifyStoringItemCategory()
        {
            TestPropertyNotSetException.ThrowIfNull(_itemCategoryMock);
            _itemCategoryRepositoryMock.VerifyStoreAsync(_itemCategoryMock.Object, Times.Once);
        }
    }
}