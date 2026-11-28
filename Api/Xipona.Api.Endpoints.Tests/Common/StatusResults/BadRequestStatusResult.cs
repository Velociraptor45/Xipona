using Xipona.Api.Domain.Common.Reasons;
using System.Net;

namespace Xipona.Api.Endpoints.Tests.Common.StatusResults;

public class BadRequestStatusResult : IStatusResult
{
    public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

    public IEnumerable<ErrorReasonCode> ExcludedErrorCodes { get; } = Enumerable.Empty<ErrorReasonCode>();
}