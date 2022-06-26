using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using System.Net;

namespace ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common.StatusResults;

public class UnprocessableEntityStatusResult : IStatusResult
{
    public UnprocessableEntityStatusResult(IEnumerable<ErrorReasonCode> excludedErrorCodes)
    {
        ExcludedErrorCodes = excludedErrorCodes;
    }

    public HttpStatusCode StatusCode => HttpStatusCode.UnprocessableEntity;
    public IEnumerable<ErrorReasonCode> ExcludedErrorCodes { get; }
}