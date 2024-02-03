using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;

public class DomainException(IReason reason) : Exception
{
    public IReason Reason { get; } = reason;
}