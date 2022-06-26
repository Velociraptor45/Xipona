using System;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Requests
{
    public interface IApiRequest
    {
        public Guid RequestId { get; }
    }
}