using Microsoft.EntityFrameworkCore;
using Xipona.Api.Repositories.Common.Converters;
using Xipona.Api.Repositories.RecipeTags.Entities;

namespace Xipona.Api.Repositories.RecipeTags.Contexts;

public class RecipeTagContext : DbContext
{
    public DbSet<RecipeTag> RecipeTags { get; set; } = null!;

    public RecipeTagContext(DbContextOptions<RecipeTagContext> options) : base(options)
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