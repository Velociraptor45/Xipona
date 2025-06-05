using Xipona.Api.Domain.Common.Reasons;
using System.Net;

namespace Xipona.Api.Endpoints.Tests.Common;

public interface IStatusResult
{
    public HttpStatusCode StatusCode { get; }
    public IEnumerable<ErrorReasonCode> ExcludedErrorCodes { get; }
}