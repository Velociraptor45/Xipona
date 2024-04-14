using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using System.Net;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;

public class CreatedStatusResult : IStatusResult
{
    public HttpStatusCode StatusCode => HttpStatusCode.Created;
    public IEnumerable<ErrorReasonCode> ExcludedErrorCodes { get; } = Enumerable.Empty<ErrorReasonCode>();
}