using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.ShoppingLists.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Models;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Stores.Models;

public class StoreTests
{
    public class ChangeName
    {
        private readonly ChangeNameFixture _fixture = new();

        [Fact]
        public void ChangeName_WithDeleted_ShouldThrow()
        {
            // Arrange
            _fixture.SetupNewName();
            _fixture.SetupDeleted();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.NewName);

            // Act
            var func = () => sut.ChangeName(_fixture.NewName);

            // Assert
            func.Should().ThrowDomainException(ErrorReasonCode.CannotModifyDeletedStore);
        }

        [Fact]
        public void ChangeName_WithValidData_ShouldChangeName()
        {
            // Arrange
            _fixture.SetupNewName();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.NewName);

            // Act
            sut.ChangeName(_fixture.NewName);

            // Assert
            sut.Name.Should().Be(_fixture.NewName);
        }

        private class ChangeNameFixture : StoreFixture
        {
            public StoreName? NewName { get; private set; }

            public void SetupNewName()
            {
                NewName = new StoreNameBuilder().Create();
            }
        }
    }

    public class ModifySectionsAsync
    {
        private readonly ModifySectionsAsyncFixture _fixture = new();

        [Fact]
        public async Task ModifySectionsAsync_WithDeleted_ShouldThrow()
        {
            // Arrange
            _fixture.SetupDeleted();
            _fixture.SetupSectionModifications();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.SectionModifications);

            // Act
            var func = () => sut.ModifySectionsAsync(
                _fixture.SectionModifications,
                _fixture.ItemModificationServiceMock.Object,
                _fixture.ShoppingListModificationServiceMock.Object);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.CannotModifyDeletedStore);
        }

        private sealed class ModifySectionsAsyncFixture : StoreFixture
        {
            public ItemModificationServiceMock ItemModificationServiceMock { get; } =
                new(MockBehavior.Strict);

            public ShoppingListModificationServiceMock ShoppingListModificationServiceMock { get; } =
                new(MockBehavior.Strict);

            public IReadOnlyCollection<SectionModification>? SectionModifications { get; private set; }

            public void SetupSectionModifications()
            {
                SectionModifications = new DomainTestBuilder<SectionModification>().CreateMany(2).ToList();
            }
        }
    }

    private abstract class StoreFixture
    {
        private readonly StoreBuilder _storeBuilder = StoreMother.Initial();

        public void SetupDeleted()
        {
            _storeBuilder.WithIsDeleted(true);
        }

        public Store CreateSut()
        {
            return _storeBuilder.Create();
        }
    }
}