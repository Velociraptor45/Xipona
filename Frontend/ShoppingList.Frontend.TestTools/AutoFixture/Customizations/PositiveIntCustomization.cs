using AutoFixture;
using AutoFixture.Kernel;

namespace ProjectHermes.ShoppingList.Frontend.TestTools.AutoFixture.Customizations;

public class PositiveIntCustomization : ICustomization
{
    private readonly int _minValue;
    private readonly int _maxValue;

    public PositiveIntCustomization(int minValue = 1, int maxValue = int.MaxValue)
    {
        if (minValue < 1)
            throw new ArgumentOutOfRangeException(nameof(minValue), $"Value ({minValue}) must be greater 0");
        if (maxValue < minValue)
            throw new ArgumentException($"MinValue ({minValue}) must be lower or equal to maxValue ({maxValue})",
                nameof(minValue));

        _minValue = minValue;
        _maxValue = maxValue;
    }

    public void Customize(IFixture fixture)
    {
        fixture.Customizations.Add(new PositiveIntSpecimenBuilder(_minValue, _maxValue));
    }

    private class PositiveIntSpecimenBuilder : ISpecimenBuilder
    {
        private readonly RandomNumericSequenceGenerator _numberGenerator;

        public PositiveIntSpecimenBuilder(int minValue, int maxValue)
        {
            _numberGenerator = new RandomNumericSequenceGenerator(minValue, maxValue);
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