using AutoFixture;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
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
            return Create(new ManufacturerDefinition());
        }

        public IManufacturer Create(ManufacturerDefinition definition)
        {
            var fixture = commonFixture.GetNewFixture();

            if (definition.Id != null)
                fixture.ConstructorArgumentFor<Manufacturer, ManufacturerId>("id", definition.Id);

            return fixture.Create<Manufacturer>();
        }
    }
}