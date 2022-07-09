using AutoFixture.Kernel;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models;

public class ItemQuantityCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customizations.Add(new ItemQuantitySpecimenBuilder());
    }

    private class ItemQuantitySpecimenBuilder : ISpecimenBuilder
    {
        private readonly RandomNumericSequenceGenerator _numberGenerator;

        public ItemQuantitySpecimenBuilder()
        {
            _numberGenerator = new RandomNumericSequenceGenerator(0, 1);
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
            return typeof(ItemQuantity) == t;
        }

        private ItemQuantity CreateInstance(ISpecimenContext context)
        {
            var quantityType = ((int)_numberGenerator.Create(typeof(int), context)).ToEnum<QuantityType>();

            switch (quantityType)
            {
                case QuantityType.Unit:
                    return new ItemQuantity(quantityType, new ItemQuantityInPacketBuilder().Create());
                case QuantityType.Weight:
                    return new ItemQuantity(quantityType, null);
                default:
                    throw new InvalidOperationException($"No setup for quantity type {quantityType}.");
            }
        }
    }
}