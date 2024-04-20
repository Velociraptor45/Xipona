using AutoFixture.Kernel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.ApplicationServices.TestKit.Common.Commands;
using ProjectHermes.Xipona.Api.ApplicationServices.TestKit.Common.Queries;
using ProjectHermes.Xipona.Api.Contracts.Common;
using ProjectHermes.Xipona.Api.Core.TestKit;
using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Endpoints.TestKit.v1.Converters;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using System.Net;
using System.Reflection;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.Common;

public abstract class ControllerQueryTestsBase<TController, TQuery, TQueryReturnType, TReturnType, TFixture>
    where TController : ControllerBase
    where TQuery : IQuery<TQueryReturnType>
    where TFixture : ControllerQueryTestsBase<TController, TQuery, TQueryReturnType, TReturnType, TFixture>
    .ControllerQueryFixtureBase
{
    protected readonly TFixture Fixture;

    protected ControllerQueryTestsBase(TFixture fixture)
    {
        Fixture = fixture;
    }

    [Fact]
    public async Task EndpointCall_WithValidData_ShouldReturnOk()
    {
        // Arrange
        Fixture.SetupParameters();
        Fixture.SetupQuery();
        Fixture.SetupQueryResult(HttpStatusCode.OK);
        Fixture.SetupDispatcherSuccess();
        Fixture.SetupExpectedResult();
        Fixture.SetupResultConverter();
        var sut = Fixture.CreateSut();

        // Act
        var result = await Fixture.ExecuteTestMethod(sut);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(Fixture.ExpectedResult);
    }

    [SkippableFact]
    public async Task EndpointCall_WithMalformedInput_ShouldReturnBadRequest()
    {
        var statusResult =
            Fixture.PossibleResults.SingleOrDefault(r => r.StatusCode == HttpStatusCode.BadRequest);

        Skip.If(statusResult is null, $"Status code 400 not relevant for endpoint {Fixture.Method.Name}");

        // Arrange
        Fixture.SetupParametersForBadRequest();
        Fixture.SetupExpectedBadRequestMessage();
        var sut = Fixture.CreateSut();

        // Act
        var result = await Fixture.ExecuteTestMethod(sut);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.Value.Should().BeEquivalentTo(Fixture.ExpectedBadRequestMessage);
    }

    [SkippableFact]
    public async Task EndpointCall_WithDomainException_ShouldReturnUnprocessableEntity()
    {
        var statusResult =
            Fixture.PossibleResults.SingleOrDefault(r => r.StatusCode == HttpStatusCode.UnprocessableEntity);

        Skip.If(statusResult is null, $"Status code 422 not relevant for endpoint {Fixture.Method.Name}");

        // Arrange
        Fixture.SetupParameters();
        Fixture.SetupQuery();
        Fixture.SetupDomainException(statusResult.ExcludedErrorCodes);
        Fixture.SetupDomainExceptionInQueryDispatcher();
        Fixture.SetupExpectedErrorContract();
        Fixture.SetupErrorConversion();
        var sut = Fixture.CreateSut();

        // Act
        var result = await Fixture.ExecuteTestMethod(sut);

        // Assert
        result.Should().BeOfType<UnprocessableEntityObjectResult>();
        var unprocessableEntity = result as UnprocessableEntityObjectResult;
        unprocessableEntity!.Value.Should().BeEquivalentTo(Fixture.ExpectedErrorContract);
    }

    #region ResponseTypes

    [SkippableFact]
    public void EndpointCall_ShouldHaveUnprocessableEntityResponseTypeAttribute()
    {
        var statusResult =
            Fixture.PossibleResults.SingleOrDefault(r => r.StatusCode == HttpStatusCode.UnprocessableEntity);

        Skip.If(statusResult is null, $"Status code 422 not relevant for endpoint {Fixture.Method.Name}");

        // Act
        var result = Fixture.GetAllResponseTypeAttributes().ToList();

        // Assert
        result.Should().ContainSingle(attr => attr.StatusCode == StatusCodes.Status422UnprocessableEntity);

        var unprocessableEntityAttribute =
            result.Single(attr => attr.StatusCode == StatusCodes.Status422UnprocessableEntity);
        unprocessableEntityAttribute.Type.Should().Be(typeof(ErrorContract));
    }

    [Fact]
    public void EndpointCall_ShouldHaveOkResponseTypeAttribute()
    {
        // Act
        var result = Fixture.GetAllResponseTypeAttributes().ToList();

        // Assert
        result.Should().ContainSingle(attr => attr.StatusCode == StatusCodes.Status200OK);

        var unprocessableEntityAttribute =
            result.Single(attr => attr.StatusCode == StatusCodes.Status200OK);
        unprocessableEntityAttribute.Type.Should().Be(typeof(TReturnType));
    }

    [SkippableFact]
    public void EndpointCall_ShouldHaveNotFoundResponseTypeAttribute()
    {
        var statusResult =
            Fixture.PossibleResults.SingleOrDefault(r => r.StatusCode == HttpStatusCode.NotFound);

        Skip.If(statusResult is null, $"Status code 404 not relevant for endpoint {Fixture.Method.Name}");

        // Act
        var result = Fixture.GetAllResponseTypeAttributes().ToList();

        // Assert
        result.Should().ContainSingle(attr => attr.StatusCode == StatusCodes.Status404NotFound);

        var unprocessableEntityAttribute =
            result.Single(attr => attr.StatusCode == StatusCodes.Status404NotFound);
        unprocessableEntityAttribute.Type.Should().Be(typeof(ErrorContract));
    }

    [SkippableFact]
    public void EndpointCall_ShouldHaveBadRequestResponseTypeAttribute()
    {
        var statusResult =
            Fixture.PossibleResults.SingleOrDefault(r => r.StatusCode == HttpStatusCode.BadRequest);

        Skip.If(statusResult is null, $"Status code 400 not relevant for endpoint {Fixture.Method.Name}");

        // Act
        var result = Fixture.GetAllResponseTypeAttributes().ToList();

        // Assert
        result.Should().ContainSingle(attr => attr.StatusCode == StatusCodes.Status400BadRequest);

        var unprocessableEntityAttribute =
            result.Single(attr => attr.StatusCode == StatusCodes.Status400BadRequest);
        unprocessableEntityAttribute.Type.Should().Be(typeof(string));
    }

    #endregion ResponseTypes

    public abstract class ControllerQueryFixtureBase
    {
        protected QueryDispatcherMock QueryDispatcherMock = new(MockBehavior.Strict);
        protected CommandDispatcherMock CommandDispatcherMock = new(MockBehavior.Strict);
        protected EndpointConvertersMock EndpointConvertersMock = new(MockBehavior.Strict);
        protected List<IStatusResult> PossibleResultsList = new();
        public abstract MethodInfo Method { get; }

        public TQuery? Query { get; protected set; }
        public TQueryReturnType? ExpectedQueryResult { get; protected set; }
        public TReturnType? ExpectedResult { get; protected set; }
        public DomainException? DomainException { get; private set; }
        public ErrorContract? ExpectedErrorContract { get; private set; }
        public string ExpectedBadRequestMessage { get; protected set; } = string.Empty;
        public IReadOnlyCollection<IStatusResult> PossibleResults => PossibleResultsList;

        public abstract TController CreateSut();

        public abstract Task<IActionResult> ExecuteTestMethod(TController sut);

        public abstract void SetupParameters();

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

        public virtual void SetupResultConverter()
        {
            TestPropertyNotSetException.ThrowIfNull(ExpectedQueryResult);
            TestPropertyNotSetException.ThrowIfNull(ExpectedResult);
            EndpointConvertersMock.SetupToContract(ExpectedQueryResult, ExpectedResult);
        }

        public IEnumerable<ProducesResponseTypeAttribute> GetAllResponseTypeAttributes()
        {
            TestPropertyNotSetException.ThrowIfNull(Method);

            return Method
                .GetCustomAttributes(typeof(ProducesResponseTypeAttribute), true)
                .OfType<ProducesResponseTypeAttribute>();
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
            EndpointConvertersMock.SetupToContract(DomainException.Reason, ExpectedErrorContract);
        }

        public virtual void SetupQueryResult(HttpStatusCode statusCode)
        {
            if (statusCode == HttpStatusCode.OK)
                ExpectedQueryResult = new DomainTestBuilder<TQueryReturnType>().Create();
        }
    }
}