using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Validations;
using ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Models;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Ports;
using ShoppingList.Api.Domain.TestKit.Shared;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ItemCategories.Services;

public class ItemCategoryValidationServiceTests
{
    [Fact]
    public async Task ValidateAsync_WithInvalidItemCategoryId_ShouldThrowDomainException()
    {
        // Arrange
        var local = new LocalFixture();
        var service = local.CreateService();
        local.SetupItemCategory();
        local.SetupFindingNoItemCategory();

        // Act
        Func<Task> function = async () => await service.ValidateAsync(local.ItemCategory.Id, default);

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
        var local = new LocalFixture();
        var service = local.CreateService();

        local.SetupItemCategory();
        local.SetupFindingItemCategory();

        // Act
        Func<Task> function = async () => await service.ValidateAsync(local.ItemCategory.Id, default);

        // Assert
        using (new AssertionScope())
        {
            await function.Should().NotThrowAsync();
        }
    }

    private class LocalFixture
    {
        public Fixture Fixture { get; }
        public CommonFixture CommonFixture { get; } = new CommonFixture();
        public ItemCategoryRepositoryMock ItemCategoryRepositoryMock { get; }
        public ItemCategory ItemCategory { get; private set; }

        public LocalFixture()
        {
            Fixture = CommonFixture.GetNewFixture();

            ItemCategoryRepositoryMock = new ItemCategoryRepositoryMock(MockBehavior.Strict);
        }

        public ItemCategoryValidationService CreateService()
        {
            return new ItemCategoryValidationService(ItemCategoryRepositoryMock.Object);
        }

        public void SetupItemCategory()
        {
            ItemCategory = new ItemCategoryBuilder().Create();
        }

        public void SetupFindingItemCategory()
        {
            ItemCategoryRepositoryMock.SetupFindByAsync(ItemCategory.Id, ItemCategory);
        }

        public void SetupFindingNoItemCategory()
        {
            ItemCategoryRepositoryMock.SetupFindByAsync(ItemCategory.Id, null);
        }
    }
}