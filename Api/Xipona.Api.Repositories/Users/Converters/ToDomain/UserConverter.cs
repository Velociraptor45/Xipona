using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.Common.Models;
using Xipona.Api.Domain.Users.Models;
using Xipona.Api.Domain.Users.Models.Factories;
using User = Xipona.Api.Repositories.Users.Entities.User;

namespace Xipona.Api.Repositories.Users.Converters.ToDomain;

public class UserConverter : IToDomainConverter<User, IUser>
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
