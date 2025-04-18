using ProjectHermes.Xipona.Api.Domain.Accounts.Models;
using ProjectHermes.Xipona.Api.Domain.Accounts.Models.Factories;
using ProjectHermes.Xipona.Api.Domain.Accounts.Ports;

namespace ProjectHermes.Xipona.Api.Domain.Accounts.Services.Creations;

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
