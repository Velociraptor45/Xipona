using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Accounts.Models;
using ProjectHermes.Xipona.Api.Domain.Common.Models;
using User = ProjectHermes.Xipona.Api.Repositories.Accounts.Entities.User;

namespace ProjectHermes.Xipona.Api.Repositories.Accounts.Converters.ToContract;

public class UserConverter : IToContractConverter<IUser, User>
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
