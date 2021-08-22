using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ShoppingList.Api.Domain.TestKit.Common;

namespace ShoppingList.Api.Domain.TestKit.Manufacturers.Models
{
    public class ManufacturerBuilder : DomainTestBuilderBase<Manufacturer>
    {
        public ManufacturerBuilder WithId(ManufacturerId id)
        {
            FillContructorWith("id", id);
            return this;
        }

        public ManufacturerBuilder WithIsDeleted(bool isDeleted)
        {
            FillContructorWith("isDeleted", isDeleted);
            return this;
        }
    }
}