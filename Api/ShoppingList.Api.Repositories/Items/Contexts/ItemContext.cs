using Microsoft.EntityFrameworkCore;
using ProjectHermes.ShoppingList.Api.Repositories.Items.Entities;

namespace ProjectHermes.ShoppingList.Api.Repositories.Items.Contexts;

public class ItemContext : DbContext
{
    public DbSet<AvailableAt> AvailableAts { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<ItemType> ItemTypes { get; set; }
    public DbSet<ItemTypeAvailableAt> ItemTypeAvailableAts { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public ItemContext(DbContextOptions<ItemContext> options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<AvailableAt>()
            .HasKey(av => new { av.ItemId, av.StoreId });
        modelBuilder.Entity<ItemTypeAvailableAt>()
            .HasKey(av => new { av.ItemTypeId, av.StoreId });
    }
}