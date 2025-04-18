using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Accounts.Models;
using ProjectHermes.Xipona.Api.Domain.Accounts.Models.Factories;
using ProjectHermes.Xipona.Api.Domain.Common.Models;
using Entities_User = ProjectHermes.Xipona.Api.Repositories.Users.Entities.User;
using User = ProjectHermes.Xipona.Api.Repositories.Users.Entities.User;

namespace ProjectHermes.Xipona.Api.Repositories.Users.Converters.ToDomain;

public class UserConverter : IToDomainConverter<Entities_User, IUser>
{
    private readonly IUserFactory _userFactory;

    public UserConverter(IUserFactory userFactory)
    {
        _userFactory = userFactory;
    }

    public IUser ToDomain(User source)
    {
        var user = (AggregateRoot)_userFactory.Create(
            new UserId(source.Id),
            source.CreatedAt);

        user.EnrichWithRowVersion(source.RowVersion);
        return (user as IUser)!;
    }
}
