using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Modifications;
using ShoppingList.Api.Domain.TestKit.Common;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Models;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ItemCategories.Models;

public class ItemCategoryTests
{
    private readonly CommonFixture _commonFixture;

    public ItemCategoryTests()
    {
        _commonFixture = new CommonFixture();
    }

    [Fact]
    public void Delete_WithValidData_ShouldMarkItemCategoryAsDeleted()
    {
        // Arrange
        var itemCategory = ItemCategoryMother.NotDeleted().Create();

        // Act
        itemCategory.Delete();

        // Assert
        using (new AssertionScope())
        {
            itemCategory.IsDeleted.Should().BeTrue();
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
        public ItemCategory CreateSut()
        {
            return new ItemCategoryBuilder().Create();
        }
    }
}