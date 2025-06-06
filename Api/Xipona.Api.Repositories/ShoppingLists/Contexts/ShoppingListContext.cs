using Microsoft.EntityFrameworkCore;
using Xipona.Api.Repositories.Common.Converters;
using Xipona.Api.Repositories.ShoppingLists.Entities;

namespace Xipona.Api.Repositories.ShoppingLists.Contexts;

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
        base.OnModelCreating(modelBuilder);
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