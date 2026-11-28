using Microsoft.EntityFrameworkCore.Design;
using Xipona.Api.Repositories.Common.Contexts;

namespace Xipona.Api.Repositories.Users.Contexts;

public class UserContextFactory : ContextFactoryBase, IDesignTimeDbContextFactory<UserContext>
{
    public UserContext CreateDbContext(string[] args)
    {
        var optionsBuilder = GetOptionBuilder<UserContext>();

        return new UserContext(optionsBuilder.Options);
    }
}
