using System;

namespace ShoppingList.Frontend.Models.Shared.Requests
{
    public interface IApiRequest
    {
        public Guid RequestId { get; }
    }
}