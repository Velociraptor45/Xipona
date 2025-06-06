using Xipona.Api.ApplicationServices.Common.Queries;
using Xipona.Api.Domain.Users.Models;

namespace Xipona.Api.ApplicationServices.Users.Queries.GetGeneralSettings;

public record GetGeneralSettingsQuery : IQuery<IGeneralSetting>;
