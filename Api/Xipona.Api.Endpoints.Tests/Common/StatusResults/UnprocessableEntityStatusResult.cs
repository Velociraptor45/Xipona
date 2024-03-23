using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using System.Net;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;

public class UnprocessableEntityStatusResult : IStatusResult
{
    public UnprocessableEntityStatusResult()
    {
        ExcludedErrorCodes = Enumerable.Empty<ErrorReasonCode>();
    }

    public UnprocessableEntityStatusResult(IEnumerable<ErrorReasonCode> excludedErrorCodes)
    {
        ExcludedErrorCodes = excludedErrorCodes;
    }

    public UnprocessableEntityStatusResult(params ErrorReasonCode[] excludedErrorCodes)
    {
        ExcludedErrorCodes = excludedErrorCodes;
    }

    public HttpStatusCode StatusCode => HttpStatusCode.UnprocessableEntity;
    public IEnumerable<ErrorReasonCode> ExcludedErrorCodes { get; }
}