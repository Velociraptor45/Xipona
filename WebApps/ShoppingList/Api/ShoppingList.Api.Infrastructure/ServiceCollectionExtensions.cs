﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShoppingList.Api.Domain.Ports;
using ShoppingList.Api.Domain.Ports.Infrastructure;
using ShoppingList.Api.Infrastructure.Adapters;
using ShoppingList.Api.Infrastructure.Entities;
using ShoppingList.Api.Infrastructure.Transaction;

namespace ShoppingList.Api.Infrastructure
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