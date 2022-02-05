using System;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;

public class DomainException : Exception
{
    public DomainException(IReason reason)
    {
        Reason = reason;
    }

    public IReason Reason { get; }
}