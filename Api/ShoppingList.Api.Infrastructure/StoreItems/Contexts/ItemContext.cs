using Microsoft.EntityFrameworkCore;
using ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Contexts
{
    public class ItemContext : DbContext
    {
        public DbSet<AvailableAt> AvailableAts { get; set; }
        public DbSet<Item> Items { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public ItemContext(DbContextOptions<ItemContext> options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}