using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using ProjectHermes.ShoppingList.Api.Infrastructure.ItemCategories.Adapters;
using ProjectHermes.ShoppingList.Api.Infrastructure.ItemCategories.Contexts;
using ProjectHermes.ShoppingList.Api.Infrastructure.Manufacturers.Adapters;
using ProjectHermes.ShoppingList.Api.Infrastructure.Manufacturers.Contexts;
using ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Adapters;
using ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Contexts;
using ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Adapters;
using ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Contexts;
using ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Adapters;
using ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Contexts;
using ProjectHermes.ShoppingList.Api.Infrastructure.Transaction;
using System;

namespace ProjectHermes.ShoppingList.Api.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            // todo: use just one connection
            services.AddDbContext<ShoppingListContext>(
                options => options.UseMySql(connectionString, new MySqlServerVersion(new Version(0, 4, 0))));
            services.AddDbContext<ItemCategoryContext>(
                options => options.UseMySql(connectionString, new MySqlServerVersion(new Version(0, 4, 0))));
            services.AddDbContext<ManufacturerContext>(
                options => options.UseMySql(connectionString, new MySqlServerVersion(new Version(0, 4, 0))));
            services.AddDbContext<ItemContext>(
                options => options.UseMySql(connectionString, new MySqlServerVersion(new Version(0, 4, 0))));
            services.AddDbContext<StoreContext>(
                options => options.UseMySql(connectionString, new MySqlServerVersion(new Version(0, 4, 0))));

            services.AddTransient<IShoppingListRepository, ShoppingListRepository>();
            services.AddTransient<IItemRepository, ItemRepository>();
            services.AddTransient<IItemCategoryRepository, ItemCategoryRepository>();
            services.AddTransient<IManufacturerRepository, ManufacturerRepository>();
            services.AddTransient<IStoreRepository, StoreRepository>();
            services.AddScoped<ITransactionGenerator, TransactionGenerator>();

            var assembly = typeof(ServiceCollectionExtensions).Assembly;
            services.AddInstancesOfGenericType(assembly, typeof(IToEntityConverter<,>));
            services.AddInstancesOfGenericType(assembly, typeof(IToDomainConverter<,>));
        }
    }
}