using ShoppingList.Core;

namespace ShoppingList.Domain.Models
{
    public class ManufacturerId : GenericPrimitive<int>
    {
        public ManufacturerId(int id)
            : base(id)
        {
        }
    }
}