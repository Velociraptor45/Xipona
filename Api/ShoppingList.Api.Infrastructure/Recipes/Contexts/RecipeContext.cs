using Microsoft.EntityFrameworkCore;
using ProjectHermes.ShoppingList.Api.Infrastructure.Recipes.Entities;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Recipes.Contexts;

public class RecipeContext : DbContext
{
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<PreparationStep> PreparationSteps { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public RecipeContext(DbContextOptions<RecipeContext> options) : base(options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }
}