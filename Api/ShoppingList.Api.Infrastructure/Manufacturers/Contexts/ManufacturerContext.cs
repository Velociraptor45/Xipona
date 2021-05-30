using Microsoft.EntityFrameworkCore;
using ProjectHermes.ShoppingList.Api.Infrastructure.Manufacturers.Entities;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Manufacturers.Contexts
{
    public class ManufacturerContext : DbContext
    {
        public DbSet<Manufacturer> Manufacturers { get; set; }

        public ManufacturerContext(DbContextOptions<ManufacturerContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}