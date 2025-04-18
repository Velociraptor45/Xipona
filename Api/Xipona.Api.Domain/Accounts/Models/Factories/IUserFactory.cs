namespace ProjectHermes.Xipona.Api.Domain.Accounts.Models.Factories;

public interface IUserFactory
{
    IUser Create(UserId id, DateTimeOffset createAt);
    IUser CreateNew(UserId id);
}