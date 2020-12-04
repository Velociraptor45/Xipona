using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Infrastructure.Adapters;
using ProjectHermes.ShoppingList.Api.Infrastructure.Entities;
using ProjectHermes.ShoppingList.Api.Infrastructure.Transaction;

namespace ProjectHermes.ShoppingList.Api.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ShoppingContext>(
                options => options.UseMySql(connectionString));

            services.AddTransient<IShoppingListRepository, ShoppingListRepository>();
            services.AddTransient<IItemRepository, ItemRepository>();
            services.AddTransient<IItemCategoryRepository, ItemCategoryRepository>();
            services.AddTransient<IManufacturerRepository, ManufacturerRepository>();
            services.AddTransient<IStoreRepository, StoreRepository>();
            services.AddScoped<ITransactionGenerator, TransactionGenerator>();
        }
    }
}