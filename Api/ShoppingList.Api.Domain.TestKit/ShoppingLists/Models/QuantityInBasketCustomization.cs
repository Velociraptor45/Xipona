using AutoFixture.Kernel;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;

public class QuantityInBasketCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customizations.Add(new QuantityInBasketSpecimenBuilder());
    }

    private class QuantityInBasketSpecimenBuilder : ISpecimenBuilder
    {
        private readonly RandomNumericSequenceGenerator _numberGenerator;

        public QuantityInBasketSpecimenBuilder()
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
            return typeof(QuantityInBasket) == t;
        }

        private QuantityInBasket CreateInstance(ISpecimenContext context)
        {
            var value = (float)_numberGenerator.Create(typeof(float), context);
            return new QuantityInBasket(value);
        }
    }
}