using FluentAssertions;
using FluentAssertions.Execution;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Fixtures;
using ShoppingList.Api.Domain.TestKit.Shared;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ItemCategories.Models
{
    public class ItemCategoryTests
    {
        private readonly CommonFixture commonFixture;
        private readonly ItemCategoryFixture itemCategoryFixture;

        public ItemCategoryTests()
        {
            commonFixture = new CommonFixture();
            itemCategoryFixture = new ItemCategoryFixture(commonFixture);
        }

        [Fact]
        public void Delete_WithValidData_ShouldMarkItemCategoryAsDeleted()
        {
            // Arrange
            var itemCategory = itemCategoryFixture.GetItemCategory(isDeleted: false);

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