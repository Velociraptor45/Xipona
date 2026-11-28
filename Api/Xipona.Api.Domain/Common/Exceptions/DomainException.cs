using Xipona.Api.Domain.Common.Reasons;

namespace Xipona.Api.Domain.Common.Exceptions;

public class DomainException(IReason reason) : Exception
{
    public IReason Reason { get; } = reason;
}