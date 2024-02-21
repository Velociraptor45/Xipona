using FluentAssertions;
using ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.Recipes.States;

public class EditedIngredientTests
{
    public class SelectedItemCategoryName
    {
        private readonly SelectedItemCategoryNameFixture _fixture = new();

        [Fact]
        public void SelectedItemCategoryName_WithItemCategoryInSelector_ShouldReturnItemCategoryName()
        {
            // Arrange
            _fixture.SetupExpectedResult();
            _fixture.SetupItemCategoryId();
            _fixture.SetupItemCategoriesContainingItemCategoryId();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.SelectedItemCategoryName;

            // Assert
            result.Should().Be(_fixture.ExpectedResult);
        }

        [Fact]
        public void SelectedItemCategoryName_WithItemCategoryNotInSelector_ShouldReturnEmptyString()
        {
            // Arrange
            _fixture.SetupExpectedResultEmpty();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.SelectedItemCategoryName;

            // Assert
            result.Should().Be(_fixture.ExpectedResult);
        }

        private sealed class SelectedItemCategoryNameFixture
        {
            private List<ItemCategorySearchResult>? _itemCategories;
            private Guid? _itemCategoryId;
            public string? ExpectedResult { get; private set; }

            public void SetupItemCategoryId()
            {
                _itemCategoryId = Guid.NewGuid();
            }

            public void SetupExpectedResult()
            {
                ExpectedResult = new DomainTestBuilder<string>().Create();
            }

            public void SetupExpectedResultEmpty()
            {
                ExpectedResult = string.Empty;
            }

            public void SetupItemCategoriesContainingItemCategoryId()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);
                TestPropertyNotSetException.ThrowIfNull(_itemCategoryId);

                var itemCategory = new DomainTestBuilder<ItemCategorySearchResult>()
                    .FillPropertyWith(i => i.Id, _itemCategoryId.Value)
                    .FillPropertyWith(i => i.Name, ExpectedResult)
                    .CreateMany(1);

                _itemCategories = new DomainTestBuilder<ItemCategorySearchResult>()
                    .CreateMany(2)
                    .Union(itemCategory)
                    .ToList();
            }

            public EditedIngredient CreateSut()
            {
                var sut = new DomainTestBuilder<EditedIngredient>().Create();

                if (_itemCategories is not null)
                {
                    sut = sut with
                    {
                        ItemCategorySelector = sut.ItemCategorySelector with
                        {
                            ItemCategories = _itemCategories
                        }
                    };
                }

                if (_itemCategoryId is not null)
                {
                    sut = sut with { ItemCategoryId = _itemCategoryId.Value };
                }

                return sut;
            }
        }
    }

    public class GetSelectedQuantityLabel
    {
        private readonly GetSelectedQuantityLabelFixture _fixture = new();

        [Fact]
        public void GetSelectedQuantityLabel_WithQuantityTypeMatchingId_ShouldReturnExpectedLabel()
        {
            // Arrange
            _fixture.SetupExpectedResult();
            var sut = GetSelectedQuantityLabelFixture.CreateSut();
            _fixture.SetupQuantityTypesMatchingId(sut.QuantityTypeId);

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);
            TestPropertyNotSetException.ThrowIfNull(_fixture.QuantityTypes);

            // Act
            var result = sut.GetSelectedQuantityLabel(_fixture.QuantityTypes);

            // Assert
            result.Should().Be(_fixture.ExpectedResult);
        }

        [Fact]
        public void GetSelectedQuantityLabel_WithQuantityTypeNotMatchingId_ShouldReturnEmptyString()
        {
            // Arrange
            _fixture.SetupExpectedResultEmpty();
            var sut = GetSelectedQuantityLabelFixture.CreateSut();
            _fixture.SetupQuantityTypes(sut.QuantityTypeId);

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);
            TestPropertyNotSetException.ThrowIfNull(_fixture.QuantityTypes);

            // Act
            var result = sut.GetSelectedQuantityLabel(_fixture.QuantityTypes);

            // Assert
            result.Should().Be(_fixture.ExpectedResult);
        }

        private sealed class GetSelectedQuantityLabelFixture
        {
            public IReadOnlyCollection<IngredientQuantityType>? QuantityTypes { get; private set; }
            public string? ExpectedResult { get; private set; }

            public void SetupExpectedResult()
            {
                ExpectedResult = new DomainTestBuilder<string>().Create();
            }

            public void SetupExpectedResultEmpty()
            {
                ExpectedResult = string.Empty;
            }

            public void SetupQuantityTypesMatchingId(int quantityTypeId)
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

                var quantityType = new List<IngredientQuantityType>
                {
                    new(quantityTypeId, ExpectedResult)
                };

                QuantityTypes = new DomainTestBuilder<IngredientQuantityType>()
                    .CreateMany(2)
                    .Union(quantityType)
                    .ToList();
            }

            public void SetupQuantityTypes(int quantityTypeId)
            {
                var types = new DomainTestBuilder<IngredientQuantityType>()
                    .CreateMany(3)
                    .ToList();

                for (var i = 0; i < types.Count; i++)
                {
                    var type = types[i];
                    if (type.Id == quantityTypeId)
                    {
                        // ensure that the id is not matching
                        types[i] = type with { Id = type.Id + 1 };
                    }
                }

                QuantityTypes = types;
            }

            public static EditedIngredient CreateSut()
            {
                return new DomainTestBuilder<EditedIngredient>().Create();
            }
        }
    }
}