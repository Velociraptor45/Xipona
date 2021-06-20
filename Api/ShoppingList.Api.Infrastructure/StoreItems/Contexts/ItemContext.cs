using Microsoft.EntityFrameworkCore;
using ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Contexts
{
    public class ItemContext : DbContext
    {
        public DbSet<AvailableAt> AvailableAts { get; set; }
        public DbSet<Item> Items { get; set; }

        public ItemContext(DbContextOptions<ItemContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}