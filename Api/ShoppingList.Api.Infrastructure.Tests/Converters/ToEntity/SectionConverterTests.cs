using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Converters.ToEntity;
using ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Entities;
using ShoppingList.Api.Core.TestKit.Converter;
using ShoppingList.Api.Domain.TestKit.Stores.Models;

namespace ShoppingList.Api.Infrastructure.Tests.Converters.ToEntity
{
    public class SectionConverterTests : ToEntityConverterTestBase<IStoreSection, Section>
    {
        protected override (IStoreSection, Section) CreateTestObjects()
        {
            var source = StoreSectionMother.Default().Create();
            var destination = GetDestination(source);

            return (source, destination);
        }

        public static Section GetDestination(IStoreSection source)
        {
            return new Section
            {
                Id = source.Id.Value,
                Name = source.Name,
                SortIndex = source.SortingIndex,
                IsDefaultSection = source.IsDefaultSection
            };
        }

        protected override void SetupServiceCollection()
        {
            serviceCollection.AddImplementationOfGenericType(typeof(SectionConverter).Assembly, typeof(IToEntityConverter<,>));
        }
    }
}