using AutoFixture;
using AutoFixture.Kernel;
using ProjectHermes.Xipona.Api.Contracts.Items.Queries.AllQuantityTypes;

namespace ProjectHermes.Xipona.Api.Contracts.TestKit.Items.Queries.AllQuantityTypes;

public class QuantityTypeInPacketContractCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customizations.Add(new QuantityTypeInPacketContractSpecimenBuilder());
    }

    private class QuantityTypeInPacketContractSpecimenBuilder : ISpecimenBuilder
    {
        private readonly RandomNumericSequenceGenerator _numberGenerator = new(0, 2);

        private readonly QuantityTypeInPacketContract[] _quantityTypes =
        [
            new QuantityTypeInPacketContract(0, "Unit", "x"),
            new QuantityTypeInPacketContract(1, "Weight", "g"),
            new QuantityTypeInPacketContract(2, "Fluid", "ml"),
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
            return typeof(QuantityTypeInPacketContract) == t;
        }

        private QuantityTypeInPacketContract CreateInstance(ISpecimenContext context)
        {
            var index = (int)_numberGenerator.Create(typeof(int), context);
            return _quantityTypes[index];
        }
    }
}