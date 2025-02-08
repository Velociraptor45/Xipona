using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using System.Net;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.Common;

public abstract class EndpointEnumerableQueryTestsBase<TQueryConverterInputType, TQuery, TQueryReturnType, TReturnType, TFixture>

    : EndpointQueryTestsBase<TQueryConverterInputType, TQuery, IEnumerable<TQueryReturnType>, List<TReturnType>, TFixture>
    where TQuery : IQuery<IEnumerable<TQueryReturnType>>
    where TFixture : EndpointEnumerableQueryTestsBase<TQueryConverterInputType, TQuery, TQueryReturnType, TReturnType, TFixture>
        .EndpointEnumerableQueryFixtureBase
{
    protected EndpointEnumerableQueryTestsBase(TFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task EndpointCall_WithValidData_ShouldReturnNoContent()
    {
        // Arrange
        Fixture.SetupParameters();
        Fixture.SetupQuery();
        Fixture.SetupQueryResult(HttpStatusCode.NoContent);
        Fixture.SetupDispatcherSuccess();
        Fixture.SetupExpectedResult();
        Fixture.SetupResultConverter();

        // Act
        var result = await Fixture.ExecuteTestMethod();

        // Assert
        result.Should().BeOfType<NoContent>();
    }

    [Fact]
    public void EndpointCall_ShouldHaveNoContentResponseTypeMetadata()
    {
        // Act
        var result = Fixture.GetAllResponseTypeMetadata().ToList();

        // Assert
        result.Should().ContainSingle(attr => attr.StatusCode == StatusCodes.Status204NoContent);

        var noContent = result.Single(attr => attr.StatusCode == StatusCodes.Status204NoContent);
        noContent.Type.Should().Be(typeof(void));
    }

    public abstract class EndpointEnumerableQueryFixtureBase : EndpointQueryFixtureBase
    {
        protected new Mock<IToContractConverter<TQueryReturnType, TReturnType>> ContractConverterMock =
            new(MockBehavior.Strict);

        public override void SetupQueryResult(HttpStatusCode statusCode)
        {
            base.SetupQueryResult(statusCode);
            if (statusCode == HttpStatusCode.NoContent)
                ExpectedQueryResult = [];
        }

        public override void SetupResultConverter()
        {
            TestPropertyNotSetException.ThrowIfNull(ExpectedQueryResult);
            TestPropertyNotSetException.ThrowIfNull(ExpectedResult);
            ContractConverterMock.Setup(x => x.ToContract(ExpectedQueryResult)).Returns(ExpectedResult);
        }
    }
}