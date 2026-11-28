using Xipona.Api.Domain.Common.Models;

namespace Xipona.Api.Domain.Users.Models;

public class User : AggregateRoot, IUser
{

    public User(UserId id, DateTimeOffset createdAt)
    {
        Id = id;
        CreatedAt = createdAt;
    }

    public UserId Id { get; }
    public DateTimeOffset CreatedAt { get; }
}
