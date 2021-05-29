using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.Stores.Fixtures;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Stores.Models
{
    public class StoreTests
    {
        private readonly CommonFixture commonFixture;
        private readonly StoreFixture storeFixture;

        public StoreTests()
        {
            commonFixture = new CommonFixture();
            storeFixture = new StoreFixture(commonFixture);
        }

        [Fact]
        public void ChangeName_WithValidData_ShouldChangeName()
        {
            // Arrange
            string newName = commonFixture.GetNewFixture().Create<string>();
            IStore store = storeFixture.GetStore();

            // Act
            store.ChangeName(newName);

            // Assert
            using (new AssertionScope())
            {
                store.Name.Should().Be(newName);
            }
        }
    }
}