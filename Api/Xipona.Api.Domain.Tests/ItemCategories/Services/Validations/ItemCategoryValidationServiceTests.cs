using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.ItemCategories.Services.Validations;
using Xipona.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using Xipona.Api.Domain.TestKit.ItemCategories.Models;
using Xipona.Api.Domain.TestKit.ItemCategories.Ports;
using Xipona.Api.TestTools.Exceptions;

namespace Xipona.Api.Domain.Tests.ItemCategories.Services.Validations;

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
        public ItemCategoryRepositoryMock ItemCategoryRepositoryMock { get; } = new(MockBehavior.Strict);
        public ItemCategory? ItemCategory { get; private set; }

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