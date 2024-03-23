using AutoFixture.Kernel;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.Recipes.Models;

public class NumberOfServingsCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customizations.Add(new NumberOfServingsSpecimenBuilder());
    }

    private class NumberOfServingsSpecimenBuilder : ISpecimenBuilder
    {
        private readonly RandomNumericSequenceGenerator _numberGenerator;

        public NumberOfServingsSpecimenBuilder()
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
            return typeof(NumberOfServings) == t;
        }

        private NumberOfServings CreateInstance(ISpecimenContext context)
        {
            var value = (int)_numberGenerator.Create(typeof(int), context);
            return new NumberOfServings(value);
        }
    }
}