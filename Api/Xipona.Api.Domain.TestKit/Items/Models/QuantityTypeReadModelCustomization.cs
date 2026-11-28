using AutoFixture.Kernel;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Items.Services.Queries.Quantities;
using Xipona.Api.Domain.TestKit.Common;

namespace Xipona.Api.Domain.TestKit.Items.Models;

public class QuantityTypeReadModelCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customizations.Add(new QuantityTypeReadModelBuilder());
    }

    private sealed class QuantityTypeReadModelBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (!MatchesType(request))
                return new NoSpecimen();

            return CreateInstance();
        }

        private static bool MatchesType(object request)
        {
            var t = request as Type;
            return typeof(QuantityTypeReadModel) == t;
        }

        private static QuantityTypeReadModel CreateInstance()
        {
            var quantityType = new DomainTestBuilder<QuantityType>().Create();
            return new QuantityTypeReadModel(quantityType);
        }
    }
}
