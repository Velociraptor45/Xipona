using Xipona.Api.Contracts.Users.Commands.Login;
using Xipona.Frontend.Infrastructure.Converters.Common;
using Xipona.Frontend.Redux.Shared.States;

namespace Xipona.Frontend.Infrastructure.Converters.Shared.ToDomain;

public class UserInfoConverter : IToDomainConverter<UserInfoContract, UserInfo>
{
    public UserInfo ToDomain(UserInfoContract source)
    {
        return new UserInfo(source.DisplayName);
    }
}
