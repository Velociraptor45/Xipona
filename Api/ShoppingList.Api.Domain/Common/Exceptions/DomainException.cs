using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using System;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(IReason reason)
        {
            Reason = reason;
        }

        public IReason Reason { get; }
    }
}