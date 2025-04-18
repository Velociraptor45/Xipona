using Microsoft.EntityFrameworkCore;
using ProjectHermes.Xipona.Api.Repositories.Accounts.Entities;

namespace ProjectHermes.Xipona.Api.Repositories.Accounts.Contexts;

public class UserContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {
    }
}
