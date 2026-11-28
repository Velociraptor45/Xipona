using Microsoft.EntityFrameworkCore;
using Xipona.Api.Repositories.Common.Converters;
using Xipona.Api.Repositories.Recipes.Entities;

namespace Xipona.Api.Repositories.Recipes.Contexts;

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

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTimeOffset))
                {
                    property.SetValueConverter(new DateTimeOffsetConverter());
                }
                else if (property.ClrType == typeof(DateTimeOffset?))
                {
                    property.SetValueConverter(new NullableDateTimeOffsetConverter());
                }
            }
        }
    }
}