using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Infrastructure.Converters.ToDomain;
using ProjectHermes.ShoppingList.Api.Infrastructure.Entities;
using ShoppingList.Api.Core.TestKit.Converter;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures;

namespace ShoppingList.Api.Infrastructure.Tests.Converters.ToDomain
{
    public class StoreItemSectionConverterTests : ToDomainConverterTestBase<Section, IStoreItemSection>
    {
        protected override (Section, IStoreItemSection) CreateTestObjects()
        {
            var commonFixture = new CommonFixture();
            var storeSectionFixture = new StoreItemSectionFixture(commonFixture);
            var destination = storeSectionFixture.Create(commonFixture.NextInt());

            var source = GetSource(destination);

            return (source, destination);
        }

        protected override void SetupServiceCollection()
        {
            AddDependencies(serviceCollection);
        }

        public static Section GetSource(IStoreItemSection destination)
        {
            return new Section()
            {
                Id = destination.Id.Value,
                Name = destination.Name,
                SortIndex = destination.SortIndex
            };
        }

        public static void AddDependencies(IServiceCollection serviceCollection)
        {
            serviceCollection.AddInstancesOfGenericType(typeof(StoreItemSectionConverter).Assembly, typeof(IToDomainConverter<,>));
            serviceCollection.AddInstancesOfNonGenericType(typeof(IStoreItemSectionFactory).Assembly, typeof(IStoreItemSectionFactory));
        }
    }
}