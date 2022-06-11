using ShoppingList.Api.Domain.TestKit.ItemCategories.Models;
using ShoppingList.Api.Domain.TestKit.Shared;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ItemCategories.Models;

public class ItemCategoryTests
{
    private readonly CommonFixture _commonFixture;

    public ItemCategoryTests()
    {
        _commonFixture = new CommonFixture();
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

    public class Modify
    {
        // TODO
    }
}