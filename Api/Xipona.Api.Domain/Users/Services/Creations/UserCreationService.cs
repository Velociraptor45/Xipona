using Xipona.Api.Domain.Users.Models;
using Xipona.Api.Domain.Users.Models.Factories;
using Xipona.Api.Domain.Users.Ports;

namespace Xipona.Api.Domain.Users.Services.Creations;

public class UserCreationService : IUserCreationService
{
    private readonly IUserFactory _userFactory;
    private readonly IUserRepository _userRepository;

    public UserCreationService(IUserFactory userFactory, IUserRepository userRepository)
    {
        _userFactory = userFactory;
        _userRepository = userRepository;
    }

    public async Task<IUser> CreateAsync(UserId id)
    {
        var user = await _userRepository.FindByAsync(id);
        if (user is not null)
            return user;

        var newUser = _userFactory.CreateNew(id);
        return await _userRepository.StoreAsync(newUser);
    }
}
