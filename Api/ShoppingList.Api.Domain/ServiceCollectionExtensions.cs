using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories;
using ProjectHermes.ShoppingList.Api.Domain.Items;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers;
using ProjectHermes.ShoppingList.Api.Domain.Shared;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists;
using ProjectHermes.ShoppingList.Api.Domain.Stores;

namespace ProjectHermes.ShoppingList.Api.Domain;

public static class ServiceCollectionExtensions
{
    public static void AddDomain(this IServiceCollection services)
    {
        services.AddItemCategories();
        services.AddManufacturers();
        services.AddShared();
        services.AddShoppingLists();
        services.AddItems();
        services.AddStores();
    }
}