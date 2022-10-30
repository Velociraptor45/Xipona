using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Items.Models.Factories;

public class ItemTypeFactoryTests
{
    public class CloneWithNewId
    {
        [Fact]
        public void CloneWithNewId_WithValidData_ShouldReturnExpectedResult()
        {
            // Arrange
            var expectedResult = new ItemTypeBuilder().Create();
            var item = new ItemType(
                ItemTypeId.New,
                expectedResult.Name,
                expectedResult.Availabilities,
                expectedResult.PredecessorId);
            var sut = new ItemTypeFactory();

            // Act
            var result = sut.CloneWithNewId(item);

            // Assert
            result.Should().BeEquivalentTo(expectedResult,
                opt => opt.Excluding(info => info.Path == "Id"));
            result.Id.Should().NotBe(item.Id);
        }
    }
}