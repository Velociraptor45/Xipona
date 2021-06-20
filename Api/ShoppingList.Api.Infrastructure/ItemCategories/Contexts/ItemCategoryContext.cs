using Microsoft.EntityFrameworkCore;
using ProjectHermes.ShoppingList.Api.Infrastructure.ItemCategories.Entities;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.ItemCategories.Contexts
{
    public class ItemCategoryContext : DbContext
    {
        public DbSet<ItemCategory> ItemCategories { get; set; }

        public ItemCategoryContext(DbContextOptions<ItemCategoryContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}