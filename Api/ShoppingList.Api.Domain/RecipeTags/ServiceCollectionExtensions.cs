using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models.Factories;

namespace ProjectHermes.ShoppingList.Api.Domain.RecipeTags;

internal static class ServiceCollectionExtensions
{
    public static void AddRecipeTags(this IServiceCollection services)
    {
        services.AddTransient<IRecipeTagFactory, RecipeTagFactory>();
    }
}