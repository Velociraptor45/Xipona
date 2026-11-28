using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.Common.Models;
using Xipona.Api.Domain.Users.Models;
using Entities_User = Xipona.Api.Repositories.Users.Entities.User;
using User = Xipona.Api.Repositories.Users.Entities.User;

namespace Xipona.Api.Repositories.Users.Converters.ToContract;

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
