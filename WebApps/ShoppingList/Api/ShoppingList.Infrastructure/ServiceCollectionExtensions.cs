using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShoppingList.Domain.Ports;
using ShoppingList.Infrastructure.Adapters;
using ShoppingList.Infrastructure.Entities;

namespace ShoppingList.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ShoppingContext>(
                options => options.UseMySql(connectionString,
                    mySqlOptions => mySqlOptions.EnableRetryOnFailure(3)));

            services.AddTransient<IShoppingListRepository, ShoppingListRepository>();
            services.AddTransient<IItemRepository, ItemRepository>();
            services.AddTransient<IItemCategoryRepository, ItemCategoryRepository>();
            services.AddTransient<IManufacturerRepository, ManufacturerRepository>();
            services.AddTransient<IStoreRepository, StoreRepository>();
        }
    }
}