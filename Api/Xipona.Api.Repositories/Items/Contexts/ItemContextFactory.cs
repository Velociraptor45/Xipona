using Microsoft.EntityFrameworkCore.Design;
using Xipona.Api.Repositories.Common.Contexts;

namespace Xipona.Api.Repositories.Items.Contexts;

public class ItemContextFactory : ContextFactoryBase, IDesignTimeDbContextFactory<ItemContext>
{
    public ItemContext CreateDbContext(string[] args)
    {
        var optionsBuilder = GetOptionBuilder<ItemContext>();

        return new ItemContext(optionsBuilder.Options);
    }
}