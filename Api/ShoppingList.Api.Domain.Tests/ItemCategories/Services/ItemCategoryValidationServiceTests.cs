using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Fixtures;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Ports;
using ShoppingList.Api.Domain.TestKit.Shared;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ItemCategories.Services
{
    public class ItemCategoryValidationServiceTests
    {
        [Fact]
        public async Task ValidateAsync_WithItemCategoryIdIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var local = new LocalFixture();
            var service = local.CreateService();

            // Act
            Func<Task> function = async () => await service.ValidateAsync(null, default);

            // Assert
            using (new AssertionScope())
            {
                await function.Should().ThrowAsync<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task ValidateAsync_WithInvalidItemCategoryId_ShouldThrowDomainException()
        {
            // Arrange
            var local = new LocalFixture();
            var service = local.CreateService();

            var itemCategoryId = new ItemCategoryId(local.CommonFixture.NextInt());

            local.ItemCategoryRepositoryMock.SetupFindByAsync(itemCategoryId, null);

            // Act
            Func<Task> function = async () => await service.ValidateAsync(itemCategoryId, default);

            // Assert
            using (new AssertionScope())
            {
                (await function.Should().ThrowAsync<DomainException>())
                    .Where(ex => ex.Reason.ErrorCode == ErrorReasonCode.ItemCategoryNotFound);
            }
        }

        [Fact]
        public async Task ValidateAsync_WithValidItemCategoryId_ShouldNotThrow()
        {
            // Arrange
            var local = new LocalFixture();
            var service = local.CreateService();

            var itemCategory = local.CreateItemCategory();

            local.ItemCategoryRepositoryMock.SetupFindByAsync(itemCategory.Id, itemCategory);

            // Act
            Func<Task> function = async () => await service.ValidateAsync(itemCategory.Id, default);

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
            public ItemCategoryFixture ItemCategoryFixture { get; }

            public LocalFixture()
            {
                Fixture = CommonFixture.GetNewFixture();

                ItemCategoryRepositoryMock = new ItemCategoryRepositoryMock(Fixture);

                ItemCategoryFixture = new ItemCategoryFixture(CommonFixture);
            }

            public ItemCategoryValidationService CreateService()
            {
                return Fixture.Create<ItemCategoryValidationService>();
            }

            public IItemCategory CreateItemCategory()
            {
                return ItemCategoryFixture.GetItemCategory();
            }
        }
    }
}