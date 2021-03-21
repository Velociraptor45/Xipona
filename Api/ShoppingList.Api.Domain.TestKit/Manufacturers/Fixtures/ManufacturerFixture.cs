using AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ShoppingList.Api.Domain.TestKit.Shared;

namespace ShoppingList.Api.Domain.TestKit.Manufacturers.Fixtures
{
    public class ManufacturerFixture
    {
        private readonly CommonFixture commonFixture;

        public ManufacturerFixture(CommonFixture commonFixture)
        {
            this.commonFixture = commonFixture;
        }

        public IManufacturer Create()
        {
            var fixture = commonFixture.GetNewFixture();

            return fixture.Create<Manufacturer>();
        }
    }
}