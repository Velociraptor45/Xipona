using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.DomainEvents;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Modifications;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ProjectHermes.Xipona.Api.Domain.TestKit.ItemCategories.Models;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.Domain.Tests.ItemCategories.Models;

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
        private readonly ModifyFixture _fixture = new();

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
            public ItemCategoryModification? Modification { get; private set; }
            public ItemCategory? ExpectedResult { get; private set; }

            public void SetupModification()
            {
                Modification = new DomainTestBuilder<ItemCategoryModification>().Create();
            }

            public void SetupExpectedResult(ItemCategory sut)
            {
                TestPropertyNotSetException.ThrowIfNull(Modification);

                ExpectedResult = new ItemCategory(sut.Id, Modification.Name, sut.IsDeleted, sut.CreatedAt);
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