using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models.Factories;
using ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Converters.ToDomain;
using ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Entities;
using ShoppingList.Api.Core.TestKit.Converter;
using ShoppingList.Api.Domain.TestKit.Stores.Models;

namespace ShoppingList.Api.Infrastructure.Tests.Converters.ToDomain
{
    public class StoreSectionConverterTests : ToDomainConverterTestBase<Section, IStoreSection>
    {
        protected override (Section, IStoreSection) CreateTestObjects()
        {
            var destination = StoreSectionMother.Default().Create();
            var source = GetSource(destination);

            return (source, destination);
        }

        protected override void SetupServiceCollection()
        {
            AddDependencies(serviceCollection);
        }

        public static Section GetSource(IStoreSection destination)
        {
            return new Section()
            {
                Id = destination.Id.Value,
                Name = destination.Name,
                SortIndex = destination.SortingIndex,
                IsDefaultSection = destination.IsDefaultSection
            };
        }

        public static void AddDependencies(IServiceCollection serviceCollection)
        {
            serviceCollection.AddInstancesOfGenericType(typeof(StoreSectionConverter).Assembly, typeof(IToDomainConverter<,>));
            serviceCollection.AddInstancesOfNonGenericType(typeof(IStoreSectionFactory).Assembly, typeof(IStoreSectionFactory));
        }
    }
}