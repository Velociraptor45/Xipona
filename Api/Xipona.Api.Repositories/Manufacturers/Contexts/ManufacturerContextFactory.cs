using Microsoft.EntityFrameworkCore.Design;
using Xipona.Api.Repositories.Common.Contexts;

namespace Xipona.Api.Repositories.Manufacturers.Contexts;

public class ManufacturerContextFactory : ContextFactoryBase, IDesignTimeDbContextFactory<ManufacturerContext>
{
    public ManufacturerContext CreateDbContext(string[] args)
    {
        var optionsBuilder = GetOptionBuilder<ManufacturerContext>();

        return new ManufacturerContext(optionsBuilder.Options);
    }
}