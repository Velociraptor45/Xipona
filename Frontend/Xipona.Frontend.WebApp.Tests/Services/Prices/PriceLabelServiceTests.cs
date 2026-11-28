using FluentAssertions;
using Moq;
using System.Globalization;
using Xipona.Frontend.Redux.Shared.States;
using Xipona.Frontend.Redux.TestKit.Common;
using Xipona.Frontend.Redux.TestKit.Shared.States;
using Xipona.Frontend.WebApp.Services.Prices;

namespace Xipona.Frontend.WebApp.Tests.Services.Prices;

public class PriceLabelServiceTests
{
    private readonly GetPriceLabelFixture _fixture = new();

    [Theory]
    [InlineData(2.015d, "/kg", true, "€", true, "2.02 €/kg")]
    [InlineData(2.015d, "", true, "€", true, "2.02 €")]
    [InlineData(2.014d, "/kg", false, "€", true, "2.01€/kg")]
    [InlineData(2.014d, "", false, "€", true, "2.01€")]
    [InlineData(4.999d, "/kg", true, "$", false, "$ 5.00 /kg")]
    [InlineData(4.999d, "", true, "$", false, "$ 5.00")]
    [InlineData(4.991d, "/kg", false, "$", false, "$4.99/kg")]
    [InlineData(4.991d, "", false, "$", false, "$4.99")]
    public void GetPriceLabel_ShouldReturnExpectedResult(decimal price, string priceLabel,
        bool spaceBetweenCurrencyAndValue, string symbol, bool isTrailing, string expectedResult)
    {
        var localCulture = Thread.CurrentThread.CurrentCulture;
        try
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            
            // Arrange
            _fixture.SetupTrailingCurrencySymbol(symbol, isTrailing);
            var sut = _fixture.CreateSut();

            // Act
            var result = sut.GetPriceLabel(price, priceLabel, spaceBetweenCurrencyAndValue);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }
        finally
        {
            Thread.CurrentThread.CurrentCulture = localCulture;
        }
    }

    private sealed class GetPriceLabelFixture : PriceLabelServiceFixture
    {
        public void SetupTrailingCurrencySymbol(string symbol, bool isTrailing)
        {
            var state = new DomainTestBuilder<SharedState>().Create();
            state = state with
            {
                Settings = state.Settings with
                {
                    GeneralSettings = state.Settings.GeneralSettings! with
                    {
                        Currency = state.Settings.GeneralSettings.Currency with
                        {
                            Symbol = symbol,
                            IsTrailing = isTrailing
                        }
                    }
                }
            };
            SharedStateMock.SetupValue(state);
        }
    }

    private abstract class PriceLabelServiceFixture
    {
        protected readonly SharedStateMock SharedStateMock = new(MockBehavior.Strict);

        public PriceLabelService CreateSut()
        {
            var sut = new PriceLabelService(SharedStateMock.Object);
            return sut;
        }
    }
}