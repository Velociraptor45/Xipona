using Microsoft.EntityFrameworkCore.Design;
using Xipona.Api.Repositories.Common.Contexts;

namespace Xipona.Api.Repositories.Users.Contexts;

public class GeneralSettingContextFactory : ContextFactoryBase, IDesignTimeDbContextFactory<GeneralSettingContext>
{
    public GeneralSettingContext CreateDbContext(string[] args)
    {
        var optionsBuilder = GetOptionBuilder<GeneralSettingContext>();

        return new GeneralSettingContext(optionsBuilder.Options);
    }
}
