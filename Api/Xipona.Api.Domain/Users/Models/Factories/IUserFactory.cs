namespace Xipona.Api.Domain.Users.Models.Factories;

public interface IUserFactory
{
    IUser Create(UserId id, DateTimeOffset createAt);
    IUser CreateNew(UserId id);
}