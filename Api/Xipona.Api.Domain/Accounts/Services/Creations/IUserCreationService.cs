using ProjectHermes.Xipona.Api.Domain.Accounts.Models;

namespace ProjectHermes.Xipona.Api.Domain.Accounts.Services.Creations;

public interface IUserCreationService
{
    Task<IUser> CreateAsync(UserId id);
}