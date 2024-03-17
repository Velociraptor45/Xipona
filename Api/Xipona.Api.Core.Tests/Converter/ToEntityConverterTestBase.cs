using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.Xipona.Api.Core.Converter;

namespace ProjectHermes.Xipona.Api.Core.Tests.Converter;

public abstract class ToEntityConverterTestBase<TSource, TDestination>
    where TDestination : class
{
    protected readonly TSource Source;
    protected readonly TDestination Destination;
    protected readonly IServiceCollection ServiceCollection;

    protected ToEntityConverterTestBase()
    {
        (Source, Destination) = CreateTestObjects();
        ServiceCollection = new ServiceCollection();
        SetupServiceCollection();
    }

    protected abstract (TSource, TDestination) CreateTestObjects();

    protected abstract void SetupServiceCollection();

    protected IToEntityConverter<TSource, TDestination> CreateConverter()
    {
        var serviceProvider = ServiceCollection.BuildServiceProvider();

        return serviceProvider.GetRequiredService<IToEntityConverter<TSource, TDestination>>();
    }

    [Fact]
    public void ToDomain_WithValidSourceObject_ShouldConvertToDestinationObject()
    {
        // Arrange
        var converter = CreateConverter();

        // Act
        TDestination result = converter.ToEntity(Source);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeEquivalentTo(Destination);
        }
    }
}