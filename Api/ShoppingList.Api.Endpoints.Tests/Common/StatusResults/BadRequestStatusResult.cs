using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using System.Net;

namespace ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common.StatusResults;

public class BadRequestStatusResult : IStatusResult
{
    public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

    public IEnumerable<ErrorReasonCode> ExcludedErrorCodes { get; } = Enumerable.Empty<ErrorReasonCode>();
}