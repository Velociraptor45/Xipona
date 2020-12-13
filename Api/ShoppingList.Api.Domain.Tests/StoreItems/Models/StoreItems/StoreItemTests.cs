using FluentAssertions;
using FluentAssertions.Execution;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Fixtures;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.StoreItems.Models.StoreItems
{
    public class StoreItemTests
    {
        private readonly CommonFixture commonFixture;
        private readonly StoreItemAvailabilityFixture storeItemAvailabilityFixture;
        private readonly StoreItemFixture storeItemFixture;

        public StoreItemTests()
        {
            commonFixture = new CommonFixture();
            storeItemAvailabilityFixture = new StoreItemAvailabilityFixture(commonFixture);
            storeItemFixture = new StoreItemFixture(storeItemAvailabilityFixture, commonFixture);
        }

        #region Delete

        [Fact]
        public void Delete_WithNotDeltedStoreItem_ShouldMarkStoreItemAsDeleted()
        {
            // Arrange
            var storeItem = storeItemFixture.GetStoreItem(new StoreItemId(commonFixture.NextInt()), isDeleted: false);

            // Act
            storeItem.Delete();

            // Assert
            using (new AssertionScope())
            {
                storeItem.IsDeleted.Should().BeTrue();
            }
        }

        #endregion Delete
    }
}