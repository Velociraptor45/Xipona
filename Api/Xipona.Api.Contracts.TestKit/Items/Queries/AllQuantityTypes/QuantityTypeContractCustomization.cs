using AutoFixture;
using AutoFixture.Kernel;
using ProjectHermes.Xipona.Api.Contracts.Items.Queries.AllQuantityTypes;

namespace ProjectHermes.Xipona.Api.Contracts.TestKit.Items.Queries.AllQuantityTypes;

public class QuantityTypeContractCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customizations.Add(new QuantityTypeContractSpecimenBuilder());
    }

    private class QuantityTypeContractSpecimenBuilder : ISpecimenBuilder
    {
        private readonly RandomNumericSequenceGenerator _numberGenerator = new(0, 1);

        private readonly QuantityTypeContract[] _quantityTypes =
        [
            new QuantityTypeContract(0, "Unit", 1, "€", "x", 1),
            new QuantityTypeContract(1, "Weight", 100, "€/kg", "g", 1000),
        ];

        public object Create(object request, ISpecimenContext context)
        {
            if (!MatchesType(request))
                return new NoSpecimen();

            return CreateInstance(context);
        }

        private bool MatchesType(object request)
        {
            var t = request as Type;
            return typeof(QuantityTypeContract) == t;
        }

        private QuantityTypeContract CreateInstance(ISpecimenContext context)
        {
            var index = (int)_numberGenerator.Create(typeof(int), context);
            return _quantityTypes[index];
        }
    }
}