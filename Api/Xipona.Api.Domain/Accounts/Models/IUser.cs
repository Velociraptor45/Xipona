namespace ProjectHermes.Xipona.Api.Domain.Accounts.Models;

public interface IUser
{
    UserId Id { get; }
    DateTimeOffset CreatedAt { get; }
}