using ShoppingList.Api.Core;

namespace ShoppingList.Api.Domain.Models
{
    public class ManufacturerId : GenericPrimitive<int>
    {
        public ManufacturerId(int id)
            : base(id)
        {
        }
    }
}