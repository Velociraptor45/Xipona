using Xipona.Api.ApplicationServices.Common.Queries;
using Xipona.Api.Domain.Users.Models;
using Xipona.Api.Domain.Users.Services.Queries;

namespace Xipona.Api.ApplicationServices.Users.Queries.GetGeneralSettings;

public class GetGeneralSettingsQueryHandler : IQueryHandler<GetGeneralSettingsQuery, IGeneralSetting>
{
    private readonly Func<CancellationToken, IGeneralSettingsQueryService> _queryServiceDelegate;

    public GetGeneralSettingsQueryHandler(Func<CancellationToken, IGeneralSettingsQueryService> queryServiceDelegate)
    {
        _queryServiceDelegate = queryServiceDelegate;
    }

    public async Task<IGeneralSetting> HandleAsync(GetGeneralSettingsQuery query, CancellationToken cancellationToken)
    {
        var queryService = _queryServiceDelegate(cancellationToken);
        return await queryService.GetGeneralSettingsAsync();
    }
}
