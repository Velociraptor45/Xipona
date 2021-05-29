using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models.Factories;
using ProjectHermes.ShoppingList.Api.Infrastructure.Converters.ToDomain;
using ShoppingList.Api.Core.TestKit.Converter;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.Stores.Fixtures;
using System.Linq;
using Entities = ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Entities;

namespace ShoppingList.Api.Infrastructure.Tests.Converters.ToDomain
{
    public class StoreConverterTests : ToDomainConverterTestBase<Entities.Store, IStore>
    {
        protected override (Entities.Store, IStore) CreateTestObjects()
        {
            var commonFixture = new CommonFixture();
            var storeFixture = new StoreFixture(commonFixture);

            var destination = storeFixture.CreateValid();
            var source = GetSource(destination, commonFixture);

            return (source, destination);
        }

        protected override void SetupServiceCollection()
        {
            AddDependencies(serviceCollection);
        }

        public static Entities.Store GetSource(IStore destination, CommonFixture commonFixture)
        {
            var sections = destination.Sections
                .Select(s => StoreSectionConverterTests.GetSource(s, commonFixture))
                .ToList();

            return new Entities.Store
            {
                Id = destination.Id.Value,
                Name = destination.Name,
                Deleted = destination.IsDeleted,
                Sections = sections
            };
        }

        public static void AddDependencies(IServiceCollection serviceCollection)
        {
            serviceCollection.AddInstancesOfGenericType(typeof(StoreConverter).Assembly, typeof(IToDomainConverter<,>));
            serviceCollection.AddInstancesOfNonGenericType(typeof(IStoreFactory).Assembly, typeof(IStoreFactory));

            StoreSectionConverterTests.AddDependencies(serviceCollection);
        }
    }
}