using Xipona.Api.Domain.Users.Models;

namespace Xipona.Api.Domain.Users.Ports;

public interface IUserRepository
{
    Task<IUser> StoreAsync(IUser model);
    Task<bool> ExistsAsync(UserId id);
    Task<IUser?> FindByAsync(UserId id);
}
