using ProjectHermes.Xipona.Api.Domain.Common.Models;

namespace ProjectHermes.Xipona.Api.Domain.Users.Services.Update;

public interface IGeneralSettingsUpdateService
{
    Task UpdateAsync(Currency currency);
}