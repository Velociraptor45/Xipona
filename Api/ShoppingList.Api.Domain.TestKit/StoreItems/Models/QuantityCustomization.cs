using AutoFixture.Kernel;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Models;

public class QuantityCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customizations.Add(new QuantitySpecimenBuilder());
    }

    private class QuantitySpecimenBuilder : ISpecimenBuilder
    {
        private readonly RandomNumericSequenceGenerator _numberGenerator;

        public QuantitySpecimenBuilder()
        {
            _numberGenerator = new RandomNumericSequenceGenerator(1, 100);
        }

        public object Create(object request, ISpecimenContext context)
        {
            if (!MatchesType(request))
                return new NoSpecimen();

            return CreateInstance(context);
        }

        private bool MatchesType(object request)
        {
            var t = request as Type;
            return typeof(Quantity) == t;
        }

        private Quantity CreateInstance(ISpecimenContext context)
        {
            var value = (float)_numberGenerator.Create(typeof(float), context);
            return new Quantity(value);
        }
    }
}