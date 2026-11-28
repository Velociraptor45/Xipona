using Xipona.Api.Domain.Common.Reasons;
using System.Net;

namespace Xipona.Api.Endpoints.Tests.Common.StatusResults;

public class NoContentStatusResult : IStatusResult
{
    public HttpStatusCode StatusCode => HttpStatusCode.NoContent;

    public IEnumerable<ErrorReasonCode> ExcludedErrorCodes { get; } = Enumerable.Empty<ErrorReasonCode>();
}