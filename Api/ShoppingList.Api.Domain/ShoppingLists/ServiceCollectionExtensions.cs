using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Domain.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.AddItems;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Conversion.ShoppingListReadModels;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Exchanges;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists;

public static class ServiceCollectionExtensions
{
    internal static void AddShoppingLists(this IServiceCollection services)
    {
        services.AddTransient<IShoppingListItemFactory, ShoppingListItemFactory>();
        services.AddTransient<IShoppingListFactory, ShoppingListFactory>();
        services.AddTransient<IShoppingListSectionFactory, ShoppingListSectionFactory>();

        services.AddTransient<IShoppingListExchangeService, ShoppingListExchangeService>();
        services.AddTransient<IAddItemToShoppingListService, AddItemToShoppingListService>();

        services.AddTransient<IShoppingListReadModelConversionService, ShoppingListReadModelConversionService>();

        services.AddTransient<Func<CancellationToken, IShoppingListModificationService>>(provider =>
        {
            var shoppingListRepository = provider.GetRequiredService<IShoppingListRepository>();
            var itemRepository = provider.GetRequiredService<IItemRepository>();
            return cancellationToken =>
                new ShoppingListModificationService(shoppingListRepository, itemRepository, cancellationToken);
        });

        services.AddTransient<Func<CancellationToken, IShoppingListQueryService>>(provider =>
        {
            var shoppingListRepository = provider.GetRequiredService<IShoppingListRepository>();
            var conversionService = provider.GetRequiredService<IShoppingListReadModelConversionService>();
            return cancellationToken =>
                new ShoppingListQueryService(shoppingListRepository, conversionService, cancellationToken);
        });
    }
}