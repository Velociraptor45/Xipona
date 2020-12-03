using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using System;

namespace ProjectHermes.ShoppingList.Api.Domain.Exceptions
{
    public class ManufacturerNotFoundException : Exception
    {
        public ManufacturerNotFoundException(ManufacturerId id)
            : base($"Manufacturer {id.Value} not found.")
        {
        }

        public ManufacturerNotFoundException(string message) : base(message)
        {
        }

        public ManufacturerNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}