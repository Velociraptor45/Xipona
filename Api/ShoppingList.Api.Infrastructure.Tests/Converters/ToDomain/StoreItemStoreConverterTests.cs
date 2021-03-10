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
using System.Linq;

namespace ShoppingList.Api.Infrastructure.Tests.Converters.ToDomain
{
    public class StoreItemStoreConverterTests : ToDomainConverterTestBase<Store, IStoreItemStore>
    {
        protected override (Store, IStoreItemStore) CreateTestObjects()
        {
            var commonFixture = new CommonFixture();
            var storeItemStoreFixture = new StoreItemStoreFixture(commonFixture);
            var destination = storeItemStoreFixture.Create();
            var source = GetSource(destination, commonFixture);

            return (source, destination);
        }

        protected override void SetupServiceCollection()
        {
            AddDependencies(serviceCollection);
        }

        public static Store GetSource(IStoreItemStore destination, CommonFixture commonFixture)
        {
            var sections = destination.Sections
                .Select(s => StoreItemSectionConverterTests.GetSource(s))
                .ToList();

            var defaultSectionIndex = commonFixture.NextInt(0, sections.Count - 1);

            return new Store
            {
                Id = destination.Id.Value,
                Name = destination.Name,
                Sections = sections
            };
        }

        public static void AddDependencies(IServiceCollection serviceCollection)
        {
            serviceCollection.AddInstancesOfGenericType(typeof(StoreItemStoreConverter).Assembly, typeof(IToDomainConverter<,>));
            serviceCollection.AddInstancesOfNonGenericType(typeof(IStoreItemStoreFactory).Assembly, typeof(IStoreItemStoreFactory));

            StoreItemSectionConverterTests.AddDependencies(serviceCollection);
        }
    }
}