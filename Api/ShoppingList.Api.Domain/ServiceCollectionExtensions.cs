using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Core.DomainEventHandlers;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories;
using ProjectHermes.ShoppingList.Api.Domain.Items;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers;
using ProjectHermes.ShoppingList.Api.Domain.Recipes;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags;
using ProjectHermes.ShoppingList.Api.Domain.Shared;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists;
using ProjectHermes.ShoppingList.Api.Domain.Stores;
using System.Reflection;

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
        services.AddRecipes();
        services.AddRecipeTags();
        services.AddStores();

        var assembly = Assembly.GetExecutingAssembly();
        services.AddImplementationOfGenericType(assembly, typeof(IDomainEventHandler<>));
    }
}