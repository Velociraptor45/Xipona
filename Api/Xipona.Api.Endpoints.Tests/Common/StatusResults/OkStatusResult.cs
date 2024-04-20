using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using System.Net;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;

public class OkStatusResult : IStatusResult
{
    public HttpStatusCode StatusCode => HttpStatusCode.OK;
    public IEnumerable<ErrorReasonCode> ExcludedErrorCodes { get; } = Enumerable.Empty<ErrorReasonCode>();
}