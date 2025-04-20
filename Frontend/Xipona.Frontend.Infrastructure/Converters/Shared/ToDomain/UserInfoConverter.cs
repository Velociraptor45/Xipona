using ProjectHermes.Xipona.Api.Contracts.Users.Commands.Login;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.Shared.States;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Shared.ToDomain;

public class UserInfoConverter : IToDomainConverter<UserInfoContract, UserInfo>
{
    public UserInfo ToDomain(UserInfoContract source)
    {
        return new UserInfo(source.DisplayName);
    }
}
