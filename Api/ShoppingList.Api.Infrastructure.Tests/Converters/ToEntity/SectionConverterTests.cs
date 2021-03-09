using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Model;
using ProjectHermes.ShoppingList.Api.Infrastructure.Converters.ToEntity;
using ProjectHermes.ShoppingList.Api.Infrastructure.Entities;
using ShoppingList.Api.Core.TestKit.Converter;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.Stores.Fixtures;

namespace ShoppingList.Api.Infrastructure.Tests.Converters.ToEntity
{
    public class SectionConverterTests : ToEntityConverterTestBase<IStoreSection, Section>
    {
        protected override (IStoreSection, Section) CreateTestObjects()
        {
            var commonFixture = new CommonFixture();
            var storeSectionFixture = new StoreSectionFixture(commonFixture);

            var source = storeSectionFixture.Create(new StoreSectionDefinition());
            var destination = GetDestination(source);

            return (source, destination);
        }

        public static Section GetDestination(IStoreSection source)
        {
            return new Section
            {
                Id = source.Id.Value,
                Name = source.Name,
                SortIndex = source.SortingIndex
            };
        }

        protected override void SetupServiceCollection()
        {
            serviceCollection.AddInstancesOfGenericType(typeof(SectionConverter).Assembly, typeof(IToEntityConverter<,>));
        }
    }
}