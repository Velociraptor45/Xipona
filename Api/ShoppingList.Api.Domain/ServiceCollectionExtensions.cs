using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Conversion.ShoppingListReadModels;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Conversion.ItemSearchReadModels;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Conversion.StoreItemReadModels;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models.Factories;
using System.Reflection;

namespace ProjectHermes.ShoppingList.Api.Domain
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDomain(this IServiceCollection services)
        {
            var assembly = typeof(ServiceCollectionExtensions).Assembly;
            services.AddHandlersForAssembly(assembly);
            services.AddInstancesOfGenericType(assembly, typeof(IToReadModelConverter<,>));

            services.AddTransient<IItemCategoryFactory, ItemCategoryFactory>();
            services.AddTransient<IManufacturerFactory, ManufacturerFactory>();
            services.AddTransient<IStoreItemFactory, StoreItemFactory>();
            services.AddTransient<IStoreItemAvailabilityFactory, StoreItemAvailabilityFactory>();
            services.AddTransient<IStoreFactory, StoreFactory>();
            services.AddTransient<IShoppingListItemFactory, ShoppingListItemFactory>();
            services.AddTransient<IShoppingListFactory, ShoppingListFactory>();
            services.AddTransient<IStoreSectionFactory, StoreSectionFactory>();
            services.AddTransient<IShoppingListSectionFactory, ShoppingListSectionFactory>();

            services.AddTransient<IAvailabilityValidationService, AvailabilityValidationService>();
            services.AddTransient<IManufacturerValidationService, ManufacturerValidationService>();
            services.AddTransient<IItemCategoryValidationService, ItemCategoryValidationService>();

            services.AddTransient<IShoppingListUpdateService, ShoppingListUpdateService>();
            services.AddTransient<IAddItemToShoppingListService, AddItemToShoppingListService>();

            services.AddTransient<IShoppingListReadModelConversionService, ShoppingListReadModelConversionService>();
            services.AddTransient<IItemSearchReadModelConversionService, ItemSearchReadModelConversionService>();
            services.AddTransient<IStoreItemReadModelConversionService, StoreItemReadModelConversionService>();

            services.AddTransient<IItemTypeFactory, ItemTypeFactory>();
        }

        public static void AddHandlersForAssembly(this IServiceCollection services, Assembly assembly)
        {
            services.AddQueryHandlersForAssembly(assembly);
            services.AddCommandHandlersForAssembly(assembly);
        }

        public static void AddQueryHandlersForAssembly(this IServiceCollection services, Assembly assembly)
        {
            var handlerType = typeof(IQueryHandler<,>);
            services.AddInstancesOfGenericType(assembly, handlerType);
        }

        public static void AddCommandHandlersForAssembly(this IServiceCollection services, Assembly assembly)
        {
            var handlerType = typeof(ICommandHandler<,>);
            services.AddInstancesOfGenericType(assembly, handlerType);
        }
    }
}