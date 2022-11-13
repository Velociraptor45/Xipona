using Microsoft.EntityFrameworkCore;
using ProjectHermes.ShoppingList.Api.Repositories.Stores.Entities;

namespace ProjectHermes.ShoppingList.Api.Repositories.Stores.Contexts;

public class StoreContext : DbContext
{
    public DbSet<Section> Sections { get; set; }
    public DbSet<Store> Stores { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public StoreContext(DbContextOptions<StoreContext> options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        : base(options)
    {
    }
}