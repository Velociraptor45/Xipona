using System;

namespace ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests
{
    public interface IApiRequest
    {
        public Guid RequestId { get; }
    }
}