using Xipona.Api.Domain.Common.Models;

namespace Xipona.Api.Domain.Users.Services.Update;

public interface IGeneralSettingsUpdateService
{
    Task UpdateAsync(Currency currency);
}