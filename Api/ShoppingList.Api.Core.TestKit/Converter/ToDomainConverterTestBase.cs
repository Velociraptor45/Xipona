using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using System;
using Xunit;

namespace ShoppingList.Api.Core.TestKit.Converter
{
    public abstract class ToDomainConverterTestBase<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {
        protected readonly TSource source;
        protected readonly TDestination destination;
        protected readonly IServiceCollection serviceCollection;

        protected ToDomainConverterTestBase()
        {
            (source, destination) = CreateTestObjects();
            serviceCollection = new ServiceCollection();
            SetupServiceCollection();
        }

        protected abstract (TSource, TDestination) CreateTestObjects();

        protected abstract void SetupServiceCollection();

        protected IToDomainConverter<TSource, TDestination> CreateConverter()
        {
            var serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider.GetRequiredService<IToDomainConverter<TSource, TDestination>>();
        }

        [Fact]
        public void ToDomain_WithSourceIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var converter = CreateConverter();

            // Act
            Action act = () => converter.ToDomain((TSource)null);

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
            TDestination result = converter.ToDomain(source);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeEquivalentTo(destination);
            }
        }
    }
}