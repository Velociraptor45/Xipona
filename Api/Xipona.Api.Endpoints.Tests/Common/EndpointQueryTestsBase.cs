using AutoFixture.Kernel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.ApplicationServices.TestKit.Common.Queries;
using ProjectHermes.Xipona.Api.Contracts.Common;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Core.TestKit;
using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using System.Net;
using System.Net.Http;
using System.Reflection;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.Common;

public abstract class EndpointQueryTestsBase<TQueryConverterInputType, TQuery, TQueryReturnType, TReturnType, TFixture>
    where TQuery : IQuery<TQueryReturnType>
    where TFixture : EndpointQueryTestsBase<TQueryConverterInputType, TQuery, TQueryReturnType, TReturnType, TFixture>
    .EndpointQueryFixtureBase
{
    protected readonly TFixture Fixture;

    protected EndpointQueryTestsBase(TFixture fixture)
    {
        Fixture = fixture;
    }

    [Fact]
    public async Task EndpointCall_WithValidData_ShouldReturnOk()
    {
        // Arrange
        Fixture.SetupParameters();
        Fixture.SetupQuery();
        Fixture.SetupQueryConverter();
        Fixture.SetupQueryResult(HttpStatusCode.OK);
        Fixture.SetupDispatcherSuccess();
        Fixture.SetupExpectedResult();
        Fixture.SetupResultConverter();

        // Act
        var result = await Fixture.ExecuteTestMethod();

        // Assert
        result.Should().BeOfType<Ok<TReturnType>>();
        var okResult = result as Ok<TReturnType>;
        okResult!.Value.Should().BeEquivalentTo(Fixture.ExpectedResult);
    }

    [SkippableFact]
    public async Task EndpointCall_WithMalformedInput_ShouldReturnBadRequest()
    {
        var statusResult =
            Fixture.PossibleResults.SingleOrDefault(r => r.StatusCode == HttpStatusCode.BadRequest);

        Skip.If(statusResult is null, $"Status code 400 not relevant for endpoint {Fixture.RoutePattern}");

        // Arrange
        Fixture.SetupParametersForBadRequest();
        Fixture.SetupExpectedBadRequestMessage();

        // Act
        var result = await Fixture.ExecuteTestMethod();

        // Assert
        result.Should().BeOfType<BadRequest<string>>();
        var badRequestResult = result as BadRequest<string>;
        badRequestResult!.Value.Should().BeEquivalentTo(Fixture.ExpectedBadRequestMessage);
    }

    [SkippableFact]
    public async Task EndpointCall_WithDomainException_ShouldReturnUnprocessableEntity()
    {
        var statusResult =
            Fixture.PossibleResults.SingleOrDefault(r => r.StatusCode == HttpStatusCode.UnprocessableEntity);

        Skip.If(statusResult is null, $"Status code 422 not relevant for endpoint {Fixture.RoutePattern}");

        // Arrange
        Fixture.SetupParameters();
        Fixture.SetupQuery();
        Fixture.SetupQueryConverter();
        Fixture.SetupDomainException(statusResult.ExcludedErrorCodes);
        Fixture.SetupDomainExceptionInQueryDispatcher();
        Fixture.SetupExpectedErrorContract();
        Fixture.SetupErrorConversion();

        // Act
        var result = await Fixture.ExecuteTestMethod();

        // Assert
        result.Should().BeOfType<UnprocessableEntity<ErrorContract>>();
        var unprocessableEntity = result as UnprocessableEntity<ErrorContract>;
        unprocessableEntity!.Value.Should().BeEquivalentTo(Fixture.ExpectedErrorContract);
    }

    #region ResponseTypes

    [SkippableFact]
    public void EndpointCall_ShouldHaveUnprocessableEntityResponseTypeMetadata()
    {
        var statusResult =
            Fixture.PossibleResults.SingleOrDefault(r => r.StatusCode == HttpStatusCode.UnprocessableEntity);

        Skip.If(statusResult is null, $"Status code 422 not relevant for endpoint {Fixture.RoutePattern}");

        // Act
        var result = Fixture.GetAllResponseTypeMetadata().ToList();

        // Assert
        result.Should().ContainSingle(attr => attr.StatusCode == StatusCodes.Status422UnprocessableEntity);

        var unprocessableEntity = result.Single(attr => attr.StatusCode == StatusCodes.Status422UnprocessableEntity);
        unprocessableEntity.Type.Should().Be(typeof(ErrorContract));
    }

    [Fact]
    public void EndpointCall_ShouldHaveOkResponseTypeMetadata()
    {
        // Act
        var result = Fixture.GetAllResponseTypeMetadata().ToList();

        // Assert
        result.Should().ContainSingle(attr => attr.StatusCode == StatusCodes.Status200OK);

        var ok = result.Single(attr => attr.StatusCode == StatusCodes.Status200OK);
        ok.Type.Should().Be(typeof(TReturnType));
    }

    [SkippableFact]
    public void EndpointCall_ShouldHaveNotFoundResponseTypeMetadata()
    {
        var statusResult =
            Fixture.PossibleResults.SingleOrDefault(r => r.StatusCode == HttpStatusCode.NotFound);

        Skip.If(statusResult is null, $"Status code 404 not relevant for endpoint {Fixture.RoutePattern}");

        // Act
        var result = Fixture.GetAllResponseTypeMetadata().ToList();

        // Assert
        result.Should().ContainSingle(attr => attr.StatusCode == StatusCodes.Status404NotFound);

        var notFound = result.Single(attr => attr.StatusCode == StatusCodes.Status404NotFound);
        notFound.Type.Should().Be(typeof(ErrorContract));
    }

    [SkippableFact]
    public void EndpointCall_ShouldHaveBadRequestResponseTypeMetadata()
    {
        var statusResult =
            Fixture.PossibleResults.SingleOrDefault(r => r.StatusCode == HttpStatusCode.BadRequest);

        Skip.If(statusResult is null, $"Status code 400 not relevant for endpoint {Fixture.RoutePattern}");

        // Act
        var result = Fixture.GetAllResponseTypeMetadata().ToList();

        // Assert
        result.Should().ContainSingle(attr => attr.StatusCode == StatusCodes.Status400BadRequest);

        var badRequest = result.Single(attr => attr.StatusCode == StatusCodes.Status400BadRequest);
        badRequest.Type.Should().Be(typeof(string));
    }

    #endregion ResponseTypes

    public abstract class EndpointQueryFixtureBase
    {
        protected QueryDispatcherMock QueryDispatcherMock = new(MockBehavior.Strict);

        protected Mock<IToContractConverter<TQueryReturnType, TReturnType>> ContractConverterMock =
            new(MockBehavior.Strict);

        protected Mock<IToContractConverter<IReason, ErrorContract>> ErrorConverterMock = new(MockBehavior.Strict);

        protected Mock<IToDomainConverter<TQueryConverterInputType, TQuery>> QueryConverterMock =
            new(MockBehavior.Strict);

        protected List<IStatusResult> PossibleResultsList = [];
        public abstract string RoutePattern { get; }
        public abstract HttpMethod HttpMethod { get; }

        public TQuery? Query { get; protected set; }
        public TQueryReturnType? ExpectedQueryResult { get; protected set; }
        public TReturnType? ExpectedResult { get; protected set; }
        public DomainException? DomainException { get; private set; }
        public ErrorContract? ExpectedErrorContract { get; private set; }
        public string ExpectedBadRequestMessage { get; protected set; } = string.Empty;
        public IReadOnlyCollection<IStatusResult> PossibleResults => PossibleResultsList;

        public abstract Task<IResult> ExecuteTestMethod();

        public abstract void SetupParameters();
        public abstract void RegisterEndpoints(WebApplication app);

        public abstract TQueryConverterInputType GetQueryConverterInput();

        public virtual void SetupParametersForBadRequest()
        {
        }

        public void SetupDispatcherSuccess()
        {
            TestPropertyNotSetException.ThrowIfNull(Query);
            TestPropertyNotSetException.ThrowIfNull(ExpectedQueryResult);
            QueryDispatcherMock.SetupDispatchAsync(Query, ExpectedQueryResult);
        }

        public abstract void SetupQuery();

        public void SetupExpectedResult()
        {
            ExpectedResult = new DomainTestBuilder<TReturnType>().Create();
        }

        public void SetupDomainExceptionInQueryDispatcher()
        {
            TestPropertyNotSetException.ThrowIfNull(DomainException);
            TestPropertyNotSetException.ThrowIfNull(Query);
            QueryDispatcherMock.SetupDispatchAsync(Query).ThrowsAsync(DomainException);
        }

        public virtual void SetupQueryConverter()
        {
            TestPropertyNotSetException.ThrowIfNull(Query);
            QueryConverterMock.Setup(x => x.ToDomain(GetQueryConverterInput())).Returns(Query);
        }

        public virtual void SetupResultConverter()
        {
            TestPropertyNotSetException.ThrowIfNull(ExpectedQueryResult);
            TestPropertyNotSetException.ThrowIfNull(ExpectedResult);
            ContractConverterMock.Setup(x => x.ToContract(ExpectedQueryResult)).Returns(ExpectedResult);
        }

        public IEnumerable<ProducesResponseTypeMetadata> GetAllResponseTypeMetadata()
        {
            var builder = WebApplication.CreateSlimBuilder();
            using var app = builder.Build();
            RegisterEndpoints(app);

            var field = typeof(WebApplication).GetField("_dataSources", BindingFlags.Instance | BindingFlags.NonPublic)!;
            var value = (List<EndpointDataSource>)field.GetValue(app)!;

            var httpMethod = HttpMethod.ToString().ToUpper();

            return value[0].Endpoints
                .First(e => ((RouteEndpoint)e).RoutePattern.RawText == RoutePattern && e.DisplayName!.Contains(httpMethod))
                .Metadata
                .Where(m => m is ProducesResponseTypeMetadata rtm && (rtm.StatusCode != 200 || rtm.Type != typeof(IResult)))
                .Cast<ProducesResponseTypeMetadata>()
                .ToList();
        }

        public void SetupExpectedErrorContract()
        {
            ExpectedErrorContract = new DomainTestBuilder<ErrorContract>().Create();
        }

        public virtual void SetupExpectedBadRequestMessage()
        {
        }

        public void SetupDomainException(IEnumerable<ErrorReasonCode> exclude)
        {
            var builder = new DomainTestBuilder<DomainException>();
            builder.Customize(new EnumExclusionCustomization<ErrorReasonCode>(exclude));
            builder.Customizations.Add(new TypeRelay(typeof(IReason), typeof(DummyReason)));

            DomainException = builder.Create();
        }

        public void SetupDomainException(ErrorReasonCode errorCode)
        {
            IReason reason = new TestBuilder<DummyReason>()
                .FillConstructorWith("errorCode", errorCode)
                .Create();
            DomainException = new DomainTestBuilder<DomainException>()
                .FillConstructorWith("reason", reason)
                .Create();
        }

        public void SetupErrorConversion()
        {
            TestPropertyNotSetException.ThrowIfNull(DomainException);
            TestPropertyNotSetException.ThrowIfNull(ExpectedErrorContract);
            ErrorConverterMock.Setup(x => x.ToContract(DomainException.Reason)).Returns(ExpectedErrorContract);
        }

        public virtual void SetupQueryResult(HttpStatusCode statusCode)
        {
            if (statusCode == HttpStatusCode.OK)
                ExpectedQueryResult = new DomainTestBuilder<TQueryReturnType>().Create();
        }
    }
}