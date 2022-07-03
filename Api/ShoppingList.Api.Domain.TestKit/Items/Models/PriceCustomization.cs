using AutoFixture.Kernel;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;

namespace ShoppingList.Api.Domain.TestKit.Items.Models;

public class PriceCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customizations.Add(new PriceSpecimenBuilder());
    }

    private class PriceSpecimenBuilder : ISpecimenBuilder
    {
        private readonly RandomNumericSequenceGenerator _numberGenerator;

        public PriceSpecimenBuilder()
        {
            _numberGenerator = new RandomNumericSequenceGenerator(1, 100);
        }

        public object Create(object request, ISpecimenContext context)
        {
            if (!MatchesType(request))
                return new NoSpecimen();

            return CreateInstance(context);
        }

        private static bool MatchesType(object request)
        {
            var t = request as Type;
            return typeof(Price) == t;
        }

        private Price CreateInstance(ISpecimenContext context)
        {
            var value = (float)_numberGenerator.Create(typeof(float), context);
            return new Price(value);
        }
    }
}