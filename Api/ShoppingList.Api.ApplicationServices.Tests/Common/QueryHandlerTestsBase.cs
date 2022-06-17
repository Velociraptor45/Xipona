using FluentAssertions;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ShoppingList.Api.TestTools.Exceptions;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Tests.Common;

public class QueryHandlerTestsBase<TQueryHandler, TQuery, TReturnType>
    where TQueryHandler : IQueryHandler<TQuery, TReturnType>
    where TQuery : IQuery<TReturnType>
{
    private readonly IQueryHandlerBaseFixture _fixture;

    protected QueryHandlerTestsBase(IQueryHandlerBaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task HandleAsync_WithValidDelegate_ShouldReturnExpectedResult()
    {
        // Arrange
        var sut = _fixture.CreateSut();
        _fixture.Setup();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Query);
        TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

        // Act
        var result = await sut.HandleAsync(_fixture.Query, default);

        // Assert
        result.Should().BeEquivalentTo(_fixture.ExpectedResult);
    }

    protected interface IQueryHandlerBaseFixture
    {
        public TQuery? Query { get; }
        public TReturnType? ExpectedResult { get; }

        public TQueryHandler CreateSut();

        public void Setup();
    }
}