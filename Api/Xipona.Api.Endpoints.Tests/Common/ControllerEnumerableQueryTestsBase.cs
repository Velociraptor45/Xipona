using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using System.Net;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.Common;

public abstract class ControllerEnumerableQueryTestsBase<TController, TQuery, TQueryReturnType, TReturnType, TFixture>

    : ControllerQueryTestsBase<TController, TQuery, IEnumerable<TQueryReturnType>, IEnumerable<TReturnType>, TFixture>
    where TController : ControllerBase
    where TQuery : IQuery<IEnumerable<TQueryReturnType>>
    where TFixture : ControllerEnumerableQueryTestsBase<TController, TQuery, TQueryReturnType, TReturnType, TFixture>
        .ControllerEnumerableQueryFixtureBase

{
    protected ControllerEnumerableQueryTestsBase(TFixture fixture) : base(fixture)
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
        var sut = Fixture.CreateSut();

        // Act
        var result = await Fixture.ExecuteTestMethod(sut);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public void EndpointCall_ShouldHaveNoContentResponseTypeAttribute()
    {
        // Act
        var result = Fixture.GetAllResponseTypeAttributes().ToList();

        // Assert
        result.Should().ContainSingle(attr => attr.StatusCode == StatusCodes.Status204NoContent);

        var unprocessableEntityAttribute =
            result.Single(attr => attr.StatusCode == StatusCodes.Status204NoContent);
        unprocessableEntityAttribute.Type.Should().Be(typeof(void));
    }

    public abstract class ControllerEnumerableQueryFixtureBase : ControllerQueryFixtureBase
    {
        public override void SetupQueryResult(HttpStatusCode statusCode)
        {
            base.SetupQueryResult(statusCode);
            if (statusCode == HttpStatusCode.NoContent)
                ExpectedQueryResult = Enumerable.Empty<TQueryReturnType>();
        }

        public override void SetupResultConverter()
        {
            TestPropertyNotSetException.ThrowIfNull(ExpectedQueryResult);
            TestPropertyNotSetException.ThrowIfNull(ExpectedResult);
            EndpointConvertersMock.SetupToContract(ExpectedQueryResult, ExpectedResult);
        }
    }
}