using Microsoft.EntityFrameworkCore;
using ProjectHermes.Xipona.Api.Repositories.Common.Converters;
using ProjectHermes.Xipona.Api.Repositories.Stores.Entities;

namespace ProjectHermes.Xipona.Api.Repositories.Stores.Contexts;

public class StoreContext : DbContext
{
    public DbSet<Section> Sections { get; set; }
    public DbSet<Store> Stores { get; set; }

    public StoreContext(DbContextOptions<StoreContext> options)
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