using Microsoft.EntityFrameworkCore;
using ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Entities;

using Entities = ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Entities;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Contexts
{
    public class ShoppingListContext : DbContext
    {
        public DbSet<ItemsOnList> ItemsOnLists { get; set; }
        public DbSet<Entities.ShoppingList> ShoppingLists { get; set; }

        public ShoppingListContext(DbContextOptions<ShoppingListContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}