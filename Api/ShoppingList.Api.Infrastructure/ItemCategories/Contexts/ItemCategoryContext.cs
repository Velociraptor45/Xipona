using Microsoft.EntityFrameworkCore;
using ProjectHermes.ShoppingList.Api.Infrastructure.ItemCategories.Entities;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.ItemCategories.Contexts;

public class ItemCategoryContext : DbContext
{
    public DbSet<ItemCategory> ItemCategories { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public ItemCategoryContext(DbContextOptions<ItemCategoryContext> options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}