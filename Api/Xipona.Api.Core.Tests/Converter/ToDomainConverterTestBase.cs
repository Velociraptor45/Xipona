using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.Xipona.Api.Core.Converter;

// ReSharper disable VirtualMemberCallInConstructor

namespace ProjectHermes.Xipona.Api.Core.Tests.Converter;

public abstract class ToDomainConverterTestBase<TSource, TDestination>
    where TSource : class
    where TDestination : class
{
    protected readonly TSource Source;
    protected readonly TDestination Destination;
    protected readonly IServiceCollection ServiceCollection;

    protected ToDomainConverterTestBase()
    {
        (Source, Destination) = CreateTestObjects();
        ServiceCollection = new ServiceCollection();
        SetupServiceCollection();
    }

    protected abstract (TSource, TDestination) CreateTestObjects();

    protected abstract void SetupServiceCollection();

    protected IToDomainConverter<TSource, TDestination> CreateConverter()
    {
        var serviceProvider = ServiceCollection.BuildServiceProvider();

        return serviceProvider.GetRequiredService<IToDomainConverter<TSource, TDestination>>();
    }

    [Fact]
    public void ToDomain_WithValidSourceObject_ShouldConvertToDestinationObject()
    {
        // Arrange
        var converter = CreateConverter();

        // Act
        TDestination result = converter.ToDomain(Source);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeEquivalentTo(Destination);
        }
    }
}