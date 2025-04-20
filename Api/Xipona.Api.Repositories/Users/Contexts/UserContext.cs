using Microsoft.EntityFrameworkCore;
using ProjectHermes.Xipona.Api.Repositories.Users.Entities;

namespace ProjectHermes.Xipona.Api.Repositories.Users.Contexts;

public class UserContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {
    }
}
