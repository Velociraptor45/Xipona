using Xipona.Api.Domain.Users.Models;

namespace Xipona.Api.Domain.Users.Services.Creations;

public interface IUserCreationService
{
    Task<IUser> CreateAsync(UserId id);
}