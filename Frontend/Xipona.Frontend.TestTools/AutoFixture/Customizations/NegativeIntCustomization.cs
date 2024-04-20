using AutoFixture;
using AutoFixture.Kernel;

namespace ProjectHermes.Xipona.Frontend.TestTools.AutoFixture.Customizations;

public class NegativeIntCustomization : ICustomization
{
    private readonly int _minValue;

    public NegativeIntCustomization(int minValue = int.MinValue)
    {
        _minValue = minValue;
    }

    public void Customize(IFixture fixture)
    {
        fixture.Customizations.Add(new NegativeIntSpecimenBuilder(_minValue));
    }

    private class NegativeIntSpecimenBuilder : ISpecimenBuilder
    {
        private readonly RandomNumericSequenceGenerator _numberGenerator;

        public NegativeIntSpecimenBuilder(int minValue)
        {
            _numberGenerator = new RandomNumericSequenceGenerator(minValue, -1);
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
            return typeof(int) == t;
        }

        private int CreateInstance(ISpecimenContext context)
        {
            return (int)_numberGenerator.Create(typeof(int), context);
        }
    }
}