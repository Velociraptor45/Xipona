using Microsoft.EntityFrameworkCore;
using ProjectHermes.ShoppingList.Api.Repositories.RecipeTags.Entities;

namespace ProjectHermes.ShoppingList.Api.Repositories.RecipeTags.Contexts;

public class RecipeTagContext : DbContext
{
    public DbSet<RecipeTag> RecipeTags { get; set; } = null!;

    public RecipeTagContext(DbContextOptions<RecipeTagContext> options) : base(options)
    {
    }
}