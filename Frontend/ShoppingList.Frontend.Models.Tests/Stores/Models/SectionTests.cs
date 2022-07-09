using FluentAssertions;
using ProjectHermes.ShoppingList.Frontend.Models.TestKit.Stores.Models;
using Xunit;

namespace ProjectHermes.ShoppingList.Frontend.Models.Tests.Stores.Models;

public class SectionTests
{
    public class AsItemSection
    {
        [Fact]
        public void AsItemSection_ShouldReturnExpectedResult()
        {
            // Arrange
            var sut = new SectionBuilder().Create();

            // Act
            var result = sut.AsItemSection();

            // Assert
            result.Id.Should().Be(sut.Id.BackendId);
            result.Name.Should().Be(sut.Name);
            result.SortingIndex.Should().Be(sut.SortingIndex);
        }
    }
}