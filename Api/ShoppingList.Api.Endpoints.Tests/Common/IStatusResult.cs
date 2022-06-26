using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using System.Net;

namespace ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common;

public interface IStatusResult
{
    public HttpStatusCode StatusCode { get; }
    public IEnumerable<ErrorReasonCode> ExcludedErrorCodes { get; }
}