using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using ProjectHermes.ShoppingList.Api.Infrastructure.Adapters;
using ProjectHermes.ShoppingList.Api.Infrastructure.Converters;
using ProjectHermes.ShoppingList.Api.Infrastructure.Entities;
using ProjectHermes.ShoppingList.Api.Infrastructure.Transaction;
using System;
using ItemCategoryModels = ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ManufacturerModels = ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using StoreModels = ProjectHermes.ShoppingList.Api.Domain.Stores.Model;

namespace ProjectHermes.ShoppingList.Api.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ShoppingContext>(
                options => options.UseMySql(connectionString, new MySqlServerVersion(new Version(0, 3, 1))));

            services.AddTransient<IShoppingListRepository, ShoppingListRepository>();
            services.AddTransient<IItemRepository, ItemRepository>();
            services.AddTransient<IItemCategoryRepository, ItemCategoryRepository>();
            services.AddTransient<IManufacturerRepository, ManufacturerRepository>();
            services.AddTransient<IStoreRepository, StoreRepository>();
            services.AddScoped<ITransactionGenerator, TransactionGenerator>();
            services.AddTransient<IStoreItemSectionReadRepository, StoreItemSectionReadRepository>();

            services.AddConverters();
        }

        private static void AddConverters(this IServiceCollection services)
        {
            services.AddTransient<IToDomainConverter<ItemCategory, ItemCategoryModels.IItemCategory>, ItemCategoryConverter>();
            services.AddTransient<IToDomainConverter<Manufacturer, ManufacturerModels.IManufacturer>, ManufacturerConverter>();

            services.AddTransient<IToDomainConverter<Store, StoreModels.IStore>, StoreConverter>();
            services.AddTransient<IToDomainConverter<Section, StoreModels.IStoreSection>, StoreSectionConverter>();

            services.AddTransient<IToDomainConverter<Entities.ShoppingList, IShoppingList>, ShoppingListConverter>();
        }
    }
}