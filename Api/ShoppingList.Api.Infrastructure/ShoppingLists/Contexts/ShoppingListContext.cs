using Microsoft.EntityFrameworkCore;
using ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Entities;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Contexts
{
    public class ShoppingListContext : DbContext
    {
        public DbSet<ItemsOnList> ItemsOnLists { get; set; }
        public DbSet<Entities.ShoppingList> ShoppingLists { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public ShoppingListContext(DbContextOptions<ShoppingListContext> options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}