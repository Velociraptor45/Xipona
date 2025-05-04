using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.Domain.Users.Models;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Users.Queries.GetGeneralSettings;

public record GetGeneralSettingsQuery : IQuery<IGeneralSetting>;
