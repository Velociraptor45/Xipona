using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using System.Net;

namespace ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common.StatusResults;

public class CreatedStatusResult : IStatusResult
{
    public HttpStatusCode StatusCode => HttpStatusCode.Created;
    public IEnumerable<ErrorReasonCode> ExcludedErrorCodes { get; } = Enumerable.Empty<ErrorReasonCode>();
}