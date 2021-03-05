using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Model;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Model.Factories;
using ProjectHermes.ShoppingList.Api.Infrastructure.Converters.ToDomain;
using ProjectHermes.ShoppingList.Api.Infrastructure.Entities;
using ShoppingList.Api.Core.TestKit.Converter;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.Stores.Fixtures;

using Entities = ProjectHermes.ShoppingList.Api.Infrastructure.Entities;

namespace ShoppingList.Api.Infrastructure.Tests.Converters.ToDomain
{
    public class StoreSectionConverterTests : ToDomainConverterTestBase<Entities.Section, IStoreSection>
    {
        protected override (Section, IStoreSection) CreateTestObjects()
        {
            var commonFixture = new CommonFixture();
            var storeSectionFixture = new StoreSectionFixture(commonFixture);
            var destination = storeSectionFixture.Create(new StoreSectionDefinition());

            var defaultSectionId = destination.IsDefaultSection
                ? destination.Id.Value
                : commonFixture.NextInt(exclude: destination.Id.Value);

            var source = new Section()
            {
                Id = destination.Id.Value,
                Name = destination.Name,
                SortIndex = destination.SortingIndex,
                Store = new Entities.Store
                {
                    DefaultSectionId = defaultSectionId
                }
            };

            return (source, destination);
        }

        protected override void SetupServiceCollection()
        {
            AddDependencies(serviceCollection);
        }

        public static void AddDependencies(IServiceCollection serviceCollection)
        {
            serviceCollection.AddInstancesOfGenericType(typeof(StoreSectionConverter).Assembly, typeof(IToDomainConverter<,>));
            serviceCollection.AddInstancesOfNonGenericType(typeof(IStoreSectionFactory).Assembly, typeof(IStoreSectionFactory));
        }
    }
}