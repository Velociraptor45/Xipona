using ProjectHermes.Xipona.Api.Domain.Accounts.Models;

namespace ProjectHermes.Xipona.Api.Domain.Accounts.Ports;

public interface IUserRepository
{
    Task<IUser> StoreAsync(IUser model);
    Task<bool> ExistsAsync(UserId id);
    Task<IUser?> FindByAsync(UserId id);
}
