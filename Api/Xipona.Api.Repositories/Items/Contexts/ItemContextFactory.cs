using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ProjectHermes.Xipona.Api.Repositories.Common.Contexts;

namespace ProjectHermes.Xipona.Api.Repositories.Items.Contexts;

public class ItemContextFactory : ContextFactoryBase, IDesignTimeDbContextFactory<ItemContext>
{
    public ItemContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ItemContext>();
        optionsBuilder.UseMySql(GetDbConnectionString(), GetVersion());

        return new ItemContext(optionsBuilder.Options);
    }
}