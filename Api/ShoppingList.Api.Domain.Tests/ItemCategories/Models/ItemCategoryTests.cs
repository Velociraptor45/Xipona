using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.DomainEvents;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ItemCategories.Models;

public class ItemCategoryTests
{
    public class Delete
    {
        private readonly DeleteFixture _fixture = new();

        [Fact]
        public void Delete_WithNotDeleted_ShouldMarkItemCategoryAsDeleted()
        {
            // Arrange
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedDomainEvent(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedDomainEvent);

            // Act
            sut.Delete();

            // Assert
            using (new AssertionScope())
            {
                sut.IsDeleted.Should().BeTrue();
                sut.DomainEvents.Should().HaveCount(1);
                var domainEvent = sut.DomainEvents.Single();
                domainEvent.Should().BeEquivalentTo(_fixture.ExpectedDomainEvent);
            }
        }

        [Fact]
        public void Delete_WithDeleted_ShouldMarkItemCategoryAsDeleted()
        {
            // Arrange
            _fixture.SetupDeleted();
            var sut = _fixture.CreateSut();

            // Act
            sut.Delete();

            // Assert
            using (new AssertionScope())
            {
                sut.IsDeleted.Should().BeTrue();
                sut.DomainEvents.Should().BeEmpty();
            }
        }

        private sealed class DeleteFixture : ItemCategoryFixture
        {
            public ItemCategoryDeletedDomainEvent? ExpectedDomainEvent { get; private set; }

            public void SetupExpectedDomainEvent(ItemCategory sut)
            {
                ExpectedDomainEvent = new ItemCategoryDeletedDomainEvent(sut.Id);
            }
        }
    }

    public class Modify
    {
        private readonly ModifyFixture _fixture;

        public Modify()
        {
            _fixture = new ModifyFixture();
        }

        [Fact]
        public void Modify_WithValidData_ShouldChangeName()
        {
            // Arrange
            _fixture.SetupModification();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResult(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            sut.Modify(_fixture.Modification);

            // Assert
            sut.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        [Fact]
        public void Modify_WithDeleted_ShouldThrow()
        {
            // Arrange
            _fixture.SetupModification();
            _fixture.SetupDeleted();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);

            // Act
            var func = () => sut.Modify(_fixture.Modification);

            // Assert
            func.Should().ThrowDomainException(ErrorReasonCode.CannotModifyDeletedItemCategory);
        }

        private class ModifyFixture : ItemCategoryFixture
        {
            public ItemCategoryModification? Modification { get; set; }
            public ItemCategory? ExpectedResult { get; set; }

            public void SetupModification()
            {
                Modification = new DomainTestBuilder<ItemCategoryModification>().Create();
            }

            public void SetupExpectedResult(ItemCategory sut)
            {
                TestPropertyNotSetException.ThrowIfNull(Modification);

                ExpectedResult = new ItemCategory(sut.Id, Modification.Name, sut.IsDeleted);
            }
        }
    }

    private abstract class ItemCategoryFixture
    {
        private ItemCategoryBuilder _itemCategoryBuilder = new();

        protected ItemCategoryFixture()
        {
            _itemCategoryBuilder.WithIsDeleted(false);
        }

        public void SetupDeleted()
        {
            _itemCategoryBuilder = _itemCategoryBuilder.WithIsDeleted(true);
        }

        public ItemCategory CreateSut()
        {
            return _itemCategoryBuilder.Create();
        }
    }
}