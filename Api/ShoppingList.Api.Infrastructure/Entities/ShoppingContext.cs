using Microsoft.EntityFrameworkCore;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Entities
{
    public class ShoppingContext : DbContext
    {
        public DbSet<AvailableAt> AvailableAts { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<ItemsOnList> ItemsOnLists { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<ShoppingList> ShoppingLists { get; set; }
        public DbSet<Store> Stores { get; set; }

        public ShoppingContext(DbContextOptions<ShoppingContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Section>()
                .HasOne(s => s.Store)
                .WithMany(store => store.Sections)
                .HasForeignKey(s => s.StoreId);

            modelBuilder.Entity<AvailableAt>()
                .HasOne(av => av.Section)
                .WithMany(section => section.DefaultItemsInSection)
                .HasForeignKey(av => av.DefaultSectionId);

            modelBuilder.Entity<ItemsOnList>()
                .HasOne(i => i.Section)
                .WithMany(section => section.ActualItemsSections)
                .HasForeignKey(i => i.SectionId);
        }
    }
}