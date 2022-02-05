using ProjectHermes.ShoppingList.Api.Core.Extensions;

namespace ShoppingList.Api.Core.Tests.Extensions;

public class ObjectExtensionsTests
{
    [Fact]
    public void ToMonoList_WithValidObject_ShouldCreateList()
    {
        // Arrange
        var obj = new TestClass();

        // Act
        var result = obj.ToMonoList();

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeEquivalentTo(new List<TestClass> { obj });
        }
    }

    [Fact]
    public void ToMonoList_WithValidStruct_ShouldCreateList()
    {
        // Arrange
        int obj = 1;

        // Act
        var result = obj.ToMonoList();

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeEquivalentTo(new List<int> { obj });
        }
    }

    private class TestClass
    {
    }
}