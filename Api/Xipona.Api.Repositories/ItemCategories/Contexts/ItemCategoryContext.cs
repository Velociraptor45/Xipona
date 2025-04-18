using Microsoft.EntityFrameworkCore;
using ProjectHermes.Xipona.Api.Repositories.ItemCategories.Entities;

namespace ProjectHermes.Xipona.Api.Repositories.ItemCategories.Contexts;

public class ItemCategoryContext : DbContext
{
    public DbSet<ItemCategory> ItemCategories { get; set; }

    public ItemCategoryContext(DbContextOptions<ItemCategoryContext> options)
        : base(options)
    {
    }
}