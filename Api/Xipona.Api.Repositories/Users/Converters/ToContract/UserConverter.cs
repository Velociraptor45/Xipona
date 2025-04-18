using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Accounts.Models;
using ProjectHermes.Xipona.Api.Domain.Common.Models;
using Entities_User = ProjectHermes.Xipona.Api.Repositories.Users.Entities.User;
using User = ProjectHermes.Xipona.Api.Repositories.Users.Entities.User;

namespace ProjectHermes.Xipona.Api.Repositories.Users.Converters.ToContract;

public class UserConverter : IToContractConverter<IUser, Entities_User>
{
    public User ToContract(IUser source)
    {
        return new User()
        {
            Id = source.Id.Value,
            CreatedAt = source.CreatedAt,
            RowVersion = ((AggregateRoot)source).RowVersion
        };
    }
}
