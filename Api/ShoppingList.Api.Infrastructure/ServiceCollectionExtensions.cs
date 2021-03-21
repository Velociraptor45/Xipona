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
using ProjectHermes.ShoppingList.Api.Infrastructure.Adapters;
using ProjectHermes.ShoppingList.Api.Infrastructure.Entities;
using ProjectHermes.ShoppingList.Api.Infrastructure.Transaction;
using System;

namespace ProjectHermes.ShoppingList.Api.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ShoppingContext>(
                options => options.UseMySql(connectionString, new MySqlServerVersion(new Version(0, 4, 0))));

            services.AddTransient<IShoppingListRepository, ShoppingListRepository>();
            services.AddTransient<IItemRepository, ItemRepository>();
            services.AddTransient<IItemCategoryRepository, ItemCategoryRepository>();
            services.AddTransient<IManufacturerRepository, ManufacturerRepository>();
            services.AddTransient<IStoreRepository, StoreRepository>();
            services.AddScoped<ITransactionGenerator, TransactionGenerator>();
            services.AddTransient<IStoreItemSectionReadRepository, StoreItemSectionReadRepository>();

            var assembly = typeof(ServiceCollectionExtensions).Assembly;
            services.AddInstancesOfGenericType(assembly, typeof(IToEntityConverter<,>));
            services.AddInstancesOfGenericType(assembly, typeof(IToDomainConverter<,>));
        }
    }
}