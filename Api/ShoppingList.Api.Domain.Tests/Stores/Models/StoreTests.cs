using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Stores.Models;

public class StoreTests
{
    private readonly CommonFixture commonFixture;

    public StoreTests()
    {
        commonFixture = new CommonFixture();
    }

    [Fact]
    public void ChangeName_WithValidData_ShouldChangeName()
    {
        // Arrange
        string newName = commonFixture.GetNewFixture().Create<string>();
        IStore store = StoreMother.Sections(3).Create();

        // Act
        store.ChangeName(newName);

        // Assert
        using (new AssertionScope())
        {
            store.Name.Should().Be(newName);
        }
    }
}