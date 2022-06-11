using FluentAssertions;
using ShoppingList.Frontend.Models.TestKit.Stores.Models;
using System.Linq;
using Xunit;

namespace ShoppingList.Frontend.Models.Tests.Stores.Models;

public class StoreTests
{
    public class AsItemStore
    {
        [Fact]
        public void AsItemStore_ShouldReturnExpectedResult()
        {
            // Arrange
            var sut = new StoreBuilder()
                .WithSections(new SectionBuilder().CreateMany(1))
                .Create();

            // Act
            var result = sut.AsItemStore();

            // Assert
            result.Id.Should().Be(sut.Id);
            result.Name.Should().Be(sut.Name);
            result.Sections.Should().HaveCount(1);

            var resultSection = result.Sections.First();
            var sutSection = sut.Sections.First();
            resultSection.Id.Should().Be(sutSection.Id.BackendId);
            resultSection.Name.Should().Be(sutSection.Name);
            resultSection.SortingIndex.Should().Be(sutSection.SortingIndex);
        }
    }
}