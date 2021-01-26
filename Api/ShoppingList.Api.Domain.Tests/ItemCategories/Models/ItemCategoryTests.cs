using FluentAssertions;
using FluentAssertions.Execution;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Fixtures;
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