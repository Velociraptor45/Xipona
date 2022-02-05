using System;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using Xunit;

namespace ShoppingList.Api.Core.TestKit.Converter;

public abstract class ToEntityConverterTestBase<TSource, TDestination>
    where TSource : class
    where TDestination : class
{
    protected readonly TSource source;
    protected readonly TDestination destination;
    protected readonly IServiceCollection serviceCollection;

    protected ToEntityConverterTestBase()
    {
        (source, destination) = CreateTestObjects();
        serviceCollection = new ServiceCollection();
        SetupServiceCollection();
    }

    protected abstract (TSource, TDestination) CreateTestObjects();

    protected abstract void SetupServiceCollection();

    protected IToEntityConverter<TSource, TDestination> CreateConverter()
    {
        var serviceProvider = serviceCollection.BuildServiceProvider();

        return serviceProvider.GetRequiredService<IToEntityConverter<TSource, TDestination>>();
    }

    [Fact]
    public void ToDomain_WithSourceIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var converter = CreateConverter();

        // Act
        Action act = () => converter.ToEntity((TSource)null);

        // Assert
        using (new AssertionScope())
        {
            act.Should().Throw<ArgumentNullException>();
        }
    }

    [Fact]
    public void ToDomain_WithValidSourceObject_ShouldConvertToDestinationObject()
    {
        // Arrange
        var converter = CreateConverter();

        // Act
        TDestination result = converter.ToEntity(source);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeEquivalentTo(destination);
        }
    }
}