using Microsoft.EntityFrameworkCore;

namespace Xipona.Api.Repositories.Common.Contexts;

/// <summary>
/// This is needed for creating migrations.
/// </summary>
public abstract class ContextFactoryBase
{
    protected static DbContextOptionsBuilder<TDbContext> GetOptionBuilder<TDbContext>() where TDbContext : DbContext
    {
        var optionsBuilder = new DbContextOptionsBuilder<TDbContext>();
        optionsBuilder.UseNpgsql();
        return optionsBuilder;
    }
}