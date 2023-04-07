using Microsoft.EntityFrameworkCore;
using ProjectHermes.ShoppingList.Api.Repositories.Recipes.Entities;

namespace ProjectHermes.ShoppingList.Api.Repositories.Recipes.Contexts;

public class RecipeContext : DbContext
{
    public DbSet<Recipe> Recipes { get; set; } = null!;
    public DbSet<Ingredient> Ingredients { get; set; } = null!;
    public DbSet<PreparationStep> PreparationSteps { get; set; } = null!;
    public DbSet<TagsForRecipe> TagsForRecipes { get; set; } = null!;

    public RecipeContext(DbContextOptions<RecipeContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<TagsForRecipe>().HasKey(t => new { t.RecipeId, t.RecipeTagId });
    }
}