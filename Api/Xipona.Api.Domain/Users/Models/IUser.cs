namespace ProjectHermes.Xipona.Api.Domain.Users.Models;

public interface IUser
{
    UserId Id { get; }
    DateTimeOffset CreatedAt { get; }
}