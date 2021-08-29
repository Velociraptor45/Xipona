using FluentAssertions;
using FluentAssertions.Execution;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Models;
using ShoppingList.Api.Domain.TestKit.Shared;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ItemCategories.Models
{
    public class ItemCategoryTests
    {
        private readonly CommonFixture commonFixture;

        public ItemCategoryTests()
        {
            commonFixture = new CommonFixture();
        }

        [Fact]
        public void Delete_WithValidData_ShouldMarkItemCategoryAsDeleted()
        {
            // Arrange
            var itemCategory = ItemCategoryMother.NotDeleted().Create();

            // Act
            itemCategory.Delete();

            // Assert
            using (new AssertionScope())
            {
                itemCategory.IsDeleted.Should().BeTrue();
            }
        }
    }
}