using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models.Factories;
using ProjectHermes.ShoppingList.Api.Infrastructure.Converters.ToDomain;
using ShoppingList.Api.Core.TestKit.Converter;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Fixtures;
using ShoppingList.Api.Domain.TestKit.Shared;
using Entities = ProjectHermes.ShoppingList.Api.Infrastructure.ItemCategories.Entities;

namespace ShoppingList.Api.Infrastructure.Tests.Converters.ToDomain
{
    public class ItemCategoryConverterTests : ToDomainConverterTestBase<Entities.ItemCategory, IItemCategory>
    {
        protected override (Entities.ItemCategory, IItemCategory) CreateTestObjects()
        {
            var commonFixture = new CommonFixture();
            var domainFixture = new ItemCategoryFixture(commonFixture);

            var destination = domainFixture.GetItemCategory();
            var source = GetSource(destination);

            return (source, destination);
        }

        protected override void SetupServiceCollection()
        {
            AddDependencies(serviceCollection);
        }

        public static Entities.ItemCategory GetSource(IItemCategory destination)
        {
            return new Entities.ItemCategory()
            {
                Id = destination.Id.Value,
                Deleted = destination.IsDeleted,
                Name = destination.Name
            };
        }

        public static void AddDependencies(IServiceCollection serviceCollection)
        {
            serviceCollection.AddInstancesOfGenericType(typeof(ItemCategoryConverter).Assembly, typeof(IToDomainConverter<,>));
            serviceCollection.AddInstancesOfNonGenericType(typeof(IItemCategoryFactory).Assembly, typeof(IItemCategoryFactory));
        }
    }
}