using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Model;
using ProjectHermes.ShoppingList.Api.Infrastructure.Converters.ToEntity;
using ShoppingList.Api.Core.TestKit.Converter;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.Stores.Fixtures;
using System.Linq;
using Entities = ProjectHermes.ShoppingList.Api.Infrastructure.Entities;

namespace ShoppingList.Api.Infrastructure.Tests.Converters.ToEntity
{
    public class StoreConverterTests : ToEntityConverterTestBase<IStore, Entities.Store>
    {
        protected override (IStore, Entities.Store) CreateTestObjects()
        {
            var commonFixture = new CommonFixture();
            var storeFixture = new StoreFixture(commonFixture);

            var source = storeFixture.CreateValid();
            var destination = GetDestination(source);

            return (source, destination);
        }

        public static Entities.Store GetDestination(IStore source)
        {
            return new Entities.Store
            {
                Id = source.Id.Value,
                Name = source.Name,
                Deleted = source.IsDeleted,
                DefaultSectionId = source.Sections.Single(s => s.IsDefaultSection).Id.Value,
                Sections = source.Sections.Select(s => SectionConverterTests.GetDestination(s)).ToList()
            };
        }

        protected override void SetupServiceCollection()
        {
            serviceCollection.AddInstancesOfGenericType(typeof(StoreConverter).Assembly, typeof(IToEntityConverter<,>));
        }
    }
}