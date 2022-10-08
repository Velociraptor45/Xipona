using AutoFixture.Kernel;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Models.Factories;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Models;

public class SectionsCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customizations.Add(new SectionsSpecimenBuilder());
    }

    private class SectionsSpecimenBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (!MatchesType(request))
                return new NoSpecimen();

            return CreateInstance();
        }

        private bool MatchesType(object request)
        {
            var t = request as Type;
            return typeof(Sections) == t;
        }

        private Sections CreateInstance()
        {
            return SectionsMother.Valid(3, new SectionFactoryMock(MockBehavior.Strict).Object);
        }
    }
}