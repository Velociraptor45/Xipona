using ProjectHermes.ShoppingList.Api.Core;

namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models
{
    public class ManufacturerId : GenericPrimitive<int>
    {
        public ManufacturerId(int id)
            : base(id)
        {
        }
    }
}