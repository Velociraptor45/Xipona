using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.Domain.Stores.Reasons;

public class StoresNotFoundReason : IReason
{
    public StoresNotFoundReason(IEnumerable<StoreId> ids)
    {
        Message = $"Stores {string.Join(", ", ids.Select(id => id.Value))} not found.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.StoresNotFound;
}