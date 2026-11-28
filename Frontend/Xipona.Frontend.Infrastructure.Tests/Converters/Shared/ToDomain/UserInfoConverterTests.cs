using Xipona.Api.Contracts.Users.Commands.Login;
using Xipona.Frontend.Infrastructure.Converters.Shared.ToDomain;
using Xipona.Frontend.Infrastructure.Tests.Common;
using Xipona.Frontend.Redux.Shared.States;

namespace Xipona.Frontend.Infrastructure.Tests.Converters.Shared.ToDomain;

public class UserInfoConverterTests : ToDomainConverterBase<UserInfoContract, UserInfo, UserInfoConverter>
{
    protected override UserInfoConverter CreateSut()
    {
        return new();
    }

    protected override void AddMapping(IMappingExpression<UserInfoContract, UserInfo> mapping)
    {
        mapping
            .ForCtorParam(nameof(UserInfo.Name), opt => opt.MapFrom(src => src.DisplayName));
    }
}
