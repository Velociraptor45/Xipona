using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using System.Net;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;

public class NotFoundStatusResult : IStatusResult
{
    public NotFoundStatusResult()
    {
        ExcludedErrorCodes = Enumerable.Empty<ErrorReasonCode>();
    }

    public NotFoundStatusResult(IEnumerable<ErrorReasonCode> excludedErrorCodes)
    {
        ExcludedErrorCodes = excludedErrorCodes;
    }

    public HttpStatusCode StatusCode => HttpStatusCode.NotFound;
    public IEnumerable<ErrorReasonCode> ExcludedErrorCodes { get; }
}