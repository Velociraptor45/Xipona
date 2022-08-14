using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Items.Models;

public class ItemTypeTests
{
    public class Update
    {
        private readonly UpdateFixture _fixture;

        public Update()
        {
            _fixture = new UpdateFixture();
        }

        [Fact]
        public void Update_WithNotAvailableAtStore_ShouldThrow()
        {
            // Arrange
            _fixture.SetupPrice();
            _fixture.SetupStoreId();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Price);

            // Act
            var func = () => sut.Update(_fixture.StoreId.Value, _fixture.Price.Value);

            // Assert
            func.Should().ThrowDomainException(ErrorReasonCode.ItemTypeAtStoreNotAvailable);
        }

        private sealed class UpdateFixture
        {
            private readonly ItemTypeBuilder _builder;

            public UpdateFixture()
            {
                _builder = new ItemTypeBuilder();
            }

            public Price? Price { get; private set; }
            public StoreId? StoreId { get; private set; }

            public ItemType CreateSut()
            {
                return _builder.Create();
            }

            public void SetupPrice()
            {
                Price = new DomainTestBuilder<Price>().Create();
            }

            public void SetupStoreId()
            {
                StoreId = Domain.Stores.Models.StoreId.New;
            }
        }
    }
}