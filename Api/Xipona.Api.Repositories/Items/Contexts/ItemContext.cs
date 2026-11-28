using Microsoft.EntityFrameworkCore;
using Xipona.Api.Repositories.Common.Converters;
using Xipona.Api.Repositories.Items.Entities;

namespace Xipona.Api.Repositories.Items.Contexts;

public class ItemContext : DbContext
{
    public DbSet<AvailableAt> AvailableAts { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<ItemType> ItemTypes { get; set; }
    public DbSet<ItemTypeAvailableAt> ItemTypeAvailableAts { get; set; }
    public DbSet<ShoppingListItemViewModel> ShoppingListItems { get; set; }

    public ItemContext(DbContextOptions<ItemContext> options)
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

        modelBuilder.Entity<ShoppingListItemViewModel>().HasNoKey().ToView("shoppinglistitemview");
        
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTimeOffset))
                {
                    property.SetValueConverter(new DateTimeOffsetConverter());
                }
                else if (property.ClrType == typeof(DateTimeOffset?))
                {
                    property.SetValueConverter(new NullableDateTimeOffsetConverter());
                }
            }
        }
    }
}