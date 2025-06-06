using Microsoft.EntityFrameworkCore;
using Xipona.Api.Repositories.Common.Converters;
using Xipona.Api.Repositories.ItemCategories.Entities;

namespace Xipona.Api.Repositories.ItemCategories.Contexts;

public class ItemCategoryContext : DbContext
{
    public DbSet<ItemCategory> ItemCategories { get; set; }

    public ItemCategoryContext(DbContextOptions<ItemCategoryContext> options)
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