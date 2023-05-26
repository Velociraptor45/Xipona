using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Validations;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.ItemCategories.Ports;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ItemCategories.Services.Validations;

public class ItemCategoryValidationServiceTests
{
    private readonly LocalFixture _fixture;

    public ItemCategoryValidationServiceTests()
    {
        _fixture = new LocalFixture();
    }

    [Fact]
    public async Task ValidateAsync_WithInvalidItemCategoryId_ShouldThrowDomainException()
    {
        // Arrange
        var service = _fixture.CreateService();
        _fixture.SetupItemCategory();
        _fixture.SetupFindingNoItemCategory();

        TestPropertyNotSetException.ThrowIfNull(_fixture.ItemCategory);

        // Act
        Func<Task> function = async () => await service.ValidateAsync(_fixture.ItemCategory.Id);

        // Assert
        using (new AssertionScope())
        {
            await function.Should().ThrowDomainExceptionAsync(ErrorReasonCode.ItemCategoryNotFound);
        }
    }

    [Fact]
    public async Task ValidateAsync_WithValidItemCategoryId_ShouldNotThrow()
    {
        // Arrange
        var service = _fixture.CreateService();

        _fixture.SetupItemCategory();
        _fixture.SetupFindingItemCategory();

        TestPropertyNotSetException.ThrowIfNull(_fixture.ItemCategory);

        // Act
        Func<Task> function = async () => await service.ValidateAsync(_fixture.ItemCategory.Id);

        // Assert
        using (new AssertionScope())
        {
            await function.Should().NotThrowAsync();
        }
    }

    private class LocalFixture
    {
        public ItemCategoryRepositoryMock ItemCategoryRepositoryMock { get; }
        public ItemCategory? ItemCategory { get; private set; }

        public LocalFixture()
        {
            ItemCategoryRepositoryMock = new ItemCategoryRepositoryMock(MockBehavior.Strict);
        }

        public ItemCategoryValidationService CreateService()
        {
            return new ItemCategoryValidationService(_ => ItemCategoryRepositoryMock.Object, default);
        }

        public void SetupItemCategory()
        {
            ItemCategory = new ItemCategoryBuilder().Create();
        }

        public void SetupFindingItemCategory()
        {
            TestPropertyNotSetException.ThrowIfNull(ItemCategory);
            ItemCategoryRepositoryMock.SetupFindActiveByAsync(ItemCategory.Id, ItemCategory);
        }

        public void SetupFindingNoItemCategory()
        {
            TestPropertyNotSetException.ThrowIfNull(ItemCategory);
            ItemCategoryRepositoryMock.SetupFindActiveByAsync(ItemCategory.Id, null);
        }
    }
}