using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Infrastructure.Converters.ToDomain;
using ProjectHermes.ShoppingList.Api.Infrastructure.Entities;
using ShoppingList.Api.Core.TestKit.Converter;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures;

using Entities = ProjectHermes.ShoppingList.Api.Infrastructure.Entities;

namespace ShoppingList.Api.Infrastructure.Tests.Converters.ToDomain
{
    public class ShoppingListStoreConverterTests : ToDomainConverterTestBase<Entities.Store, IShoppingListStore>
    {
        protected override (Store, IShoppingListStore) CreateTestObjects()
        {
            var commonFixture = new CommonFixture();
            var shoppingListStoreFixture = new ShoppingListStoreFixture(commonFixture);

            var destination = shoppingListStoreFixture.CreateValid();
            var source = GetSource(destination);

            return (source, destination);
        }

        protected override void SetupServiceCollection()
        {
            AddDependencies(serviceCollection);
        }

        public static Store GetSource(IShoppingListStore destination)
        {
            return new Store
            {
                Id = destination.Id.Value,
                Name = destination.Name,
                Deleted = destination.IsDeleted
            };
        }

        public static void AddDependencies(IServiceCollection serviceCollection)
        {
            serviceCollection.AddInstancesOfGenericType(typeof(ShoppingListStoreConverter).Assembly, typeof(IToDomainConverter<,>));
            serviceCollection.AddInstancesOfNonGenericType(typeof(IShoppingListStoreFactory).Assembly, typeof(IShoppingListStoreFactory));
        }
    }
}