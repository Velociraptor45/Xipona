using FluentAssertions;
using Moq;
using Xipona.Frontend.Redux.Shared.States;
using Xipona.Frontend.Redux.ShoppingList.States;
using Xipona.Frontend.Redux.ShoppingList.States.Comparer;
using Xipona.Frontend.Redux.TestKit.Common;
using Xipona.Frontend.Redux.TestKit.ShoppingList.States;
using Xipona.Frontend.TestTools.AutoFixture.Builder;
using Xipona.Frontend.TestTools.Exceptions;
using Xipona.Frontend.WebApp.Services.Prices;

namespace Xipona.Frontend.WebApp.Tests.Services.Prices;

public class ItemPriceCalculationServiceTests
{
    public sealed class CalculatePrice
    {

        private readonly CalculatePriceFixture _fixture = new();

        [Theory]
        [InlineData(1, 2, 2, 4)]
        [InlineData(1, 1.5f, 1, 1.5)]
        [InlineData(1000, 1.5f, 300, 0.45)]
        [InlineData(1000, 1.5f, 512, 0.77)] // round up above .5
        [InlineData(1000, 1.5f, 470, 0.71)] // round up at .5
        [InlineData(1000, 1.5f, 462, 0.69)] // round down
        public void CalculatePrice_ShouldReturnExpectedResult(int quantityNormalizer, decimal pricePerQuantity,
            float quantity, decimal expectedResult)
        {
            // Arrange
            _fixture.SetupQuantityType(quantityNormalizer);
            _fixture.SetupValidQuantityTypeId();
            _fixture.SetupStateWithQuantityTypes();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.QuantityTypeId);

            // Act
            var result = sut.CalculatePrice(_fixture.QuantityTypeId.Value, pricePerQuantity, quantity);

            // Assert
            result.Should().Be(expectedResult);
        }

        [Fact]
        public void CalculatePrice_WithInvalidQuantityId_ShouldThrow()
        {
            // Arrange
            _fixture.SetupInvalidQuantityTypeId();
            _fixture.SetupStateWithoutQuantityTypes();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.QuantityTypeId);

            // Act
            var func = () => sut.CalculatePrice(_fixture.QuantityTypeId.Value, 1, 1);

            // Assert
            func.Should().ThrowExactly<InvalidOperationException>()
                .WithMessage($"Quantity type {_fixture.QuantityTypeId.Value} not recognized.");
        }

        private class CalculatePriceFixture : ItemPriceCalculationServiceFixture
        {
            private List<QuantityType>? _quantityTypes;

            public int? QuantityTypeId { get; private set; }

            public void SetupStateWithQuantityTypes()
            {
                TestPropertyNotSetException.ThrowIfNull(_quantityTypes);
                var state = new DomainTestBuilder<ShoppingListState>().Create() with { QuantityTypes = _quantityTypes };
                ShoppingListStateMock.SetupValue(state);
            }

            public void SetupStateWithoutQuantityTypes()
            {
                var state = new DomainTestBuilder<ShoppingListState>().Create();
                ShoppingListStateMock.SetupValue(state);
            }

            public void SetupQuantityType(int quantityNormalizer)
            {
                _quantityTypes =
                [
                    new DomainTestBuilder<QuantityType>().Create() with { QuantityNormalizer = quantityNormalizer }
                ];
            }

            public void SetupValidQuantityTypeId()
            {
                TestPropertyNotSetException.ThrowIfNull(_quantityTypes);

                QuantityTypeId = _quantityTypes.First().Id;
            }

            public void SetupInvalidQuantityTypeId()
            {
                QuantityTypeId = new IntBuilder().CreatePositive();
            }
        }
    }

    public sealed class GetInBasketPrice
    {
        private readonly GetInBasketPriceFixture _fixture = new();

        [Fact]
        public void GetInBasketPrice_WithoutDiscounts_IncludeDiscounts_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupShoppingListWithoutDiscounts();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingList);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.GetInBasketPrice(_fixture.ShoppingList, true);

            // Assert
            result.Should().Be(_fixture.ExpectedResult);
        }

        [Fact]
        public void GetInBasketPrice_WithoutDiscounts_NotIncludeDiscounts_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupShoppingListWithoutDiscounts();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingList);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.GetInBasketPrice(_fixture.ShoppingList, false);

            // Assert
            result.Should().Be(_fixture.ExpectedResult);
        }

        [Fact]
        public void GetInBasketPrice_WithDiscounts_IncludeDiscounts_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupShoppingListWithDiscounts(true);
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingList);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.GetInBasketPrice(_fixture.ShoppingList, true);

            // Assert
            result.Should().Be(_fixture.ExpectedResult);
        }

        [Fact]
        public void GetInBasketPrice_WithDiscounts_NotIncludeDiscounts_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupShoppingListWithDiscounts(false);
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingList);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.GetInBasketPrice(_fixture.ShoppingList, false);

            // Assert
            result.Should().Be(_fixture.ExpectedResult);
        }

        private class GetInBasketPriceFixture : ItemPriceCalculationServiceFixture
        {
            public ShoppingListModel? ShoppingList { get; private set; }
            public decimal? ExpectedResult { get; private set; }

            public void SetupShoppingListWithoutDiscounts()
            {
                ShoppingListItem[] items =
                [
                    new DomainTestBuilder<ShoppingListItem>().Create() with
                    {
                        IsInBasket = true,
                        Quantity = 2,
                        QuantityType = new DomainTestBuilder<QuantityType>().Create() with
                        {
                            QuantityNormalizer = 1
                        },
                        PricePerQuantity = 2.99m
                    },
                    new DomainTestBuilder<ShoppingListItem>().Create() with
                    {
                        IsInBasket = true,
                        Quantity = 350,
                        QuantityType = new DomainTestBuilder<QuantityType>().Create() with
                        {
                            QuantityNormalizer = 100
                        },
                        PricePerQuantity = 19.99m
                    },
                    new DomainTestBuilder<ShoppingListItem>().Create() with
                    {
                        IsInBasket = false,
                        Quantity = 480,
                        QuantityType = new DomainTestBuilder<QuantityType>().Create() with
                        {
                            QuantityNormalizer = 100
                        },
                        PricePerQuantity = 7.99m
                    }
                ];

                ShoppingList = new DomainTestBuilder<ShoppingListModel>().Create() with
                {
                    Discounts = [],
                    Sections = new SortedSet<ShoppingListSection>(
                        [
                            new DomainTestBuilder<ShoppingListSection>().Create() with
                            {
                                Items = items
                            }
                        ],
                        new SortingIndexComparer())
                };

                ExpectedResult = 5.98m + 69.97m; // item1 + item2
            }

            public void SetupShoppingListWithDiscounts(bool includeDiscountsInExpectedPrice)
            {
                ShoppingListItem[] items =
                [
                    new DomainTestBuilder<ShoppingListItem>().Create() with
                    {
                        IsInBasket = true,
                        Quantity = 2,
                        QuantityType = new DomainTestBuilder<QuantityType>().Create() with
                        {
                            QuantityNormalizer = 1
                        },
                        PricePerQuantity = 2.99m
                    },
                    new DomainTestBuilder<ShoppingListItem>().Create() with
                    {
                        IsInBasket = true,
                        Quantity = 350,
                        QuantityType = new DomainTestBuilder<QuantityType>().Create() with
                        {
                            QuantityNormalizer = 100
                        },
                        PricePerQuantity = 19.99m
                    },
                    new DomainTestBuilder<ShoppingListItem>().Create() with
                    {
                        IsInBasket = false,
                        Quantity = 480,
                        QuantityType = new DomainTestBuilder<QuantityType>().Create() with
                        {
                            QuantityNormalizer = 100
                        },
                        PricePerQuantity = 7.99m
                    }
                ];

                ShoppingList = new DomainTestBuilder<ShoppingListModel>().Create() with
                {
                    Discounts =
                    [
                        new DomainTestBuilder<ShoppingListDiscount>().Create() with
                        {
                            DiscountValue = 1m,
                            Type = ShoppingListDiscountType.Price
                        },
                        new DomainTestBuilder<ShoppingListDiscount>().Create() with
                        {
                            DiscountValue = 10m,
                            Type = ShoppingListDiscountType.Percentage
                        }
                    ],
                    Sections = new SortedSet<ShoppingListSection>(
                        [
                            new DomainTestBuilder<ShoppingListSection>().Create() with
                            {
                                Items = items
                            }
                        ],
                        new SortingIndexComparer())
                };

                if (includeDiscountsInExpectedPrice)
                    ExpectedResult = (5.98m + 69.97m - 1m) * 0.9m; // (item1 + item2 - discount1) * (1 - discount2)
                else
                    ExpectedResult = 5.98m + 69.97m; // item1 + item2
            }
        }
    }

    private abstract class ItemPriceCalculationServiceFixture
    {
        protected readonly ShoppingListStateMock ShoppingListStateMock = new(MockBehavior.Strict);

        public ItemPriceCalculationService CreateSut()
        {
            var sut = new ItemPriceCalculationService(ShoppingListStateMock.Object);
            return sut;
        }
    }
}