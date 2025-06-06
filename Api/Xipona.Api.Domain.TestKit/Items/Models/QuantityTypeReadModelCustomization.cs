using AutoFixture.Kernel;
using Xipona.Api.Core.TestKit;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Items.Services.Queries.Quantities;
using Xipona.Api.Domain.TestKit.Common;
using Xipona.Api.Domain.Users.Models;

namespace Xipona.Api.Domain.TestKit.Items.Models;

public class QuantityTypeReadModelCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customizations.Add(new QuantityTypeReadModelBuilder());
    }

    public class QuantityTypeReadModelBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (!MatchesType(request))
                return new NoSpecimen();

            return CreateInstance(context);
        }

        private static bool MatchesType(object request)
        {
            var t = request as Type;
            return typeof(QuantityTypeReadModel) == t;
        }

        private static QuantityTypeReadModel CreateInstance(ISpecimenContext context)
        {
            var cacheMock = new MemoryCacheMock(MockBehavior.Strict);

            var settings = new DomainTestBuilder<GeneralSetting>().Create();
            cacheMock.SetupTryGetValue("GeneralSettings", settings, true);

            var quantityType = new DomainTestBuilder<QuantityType>().Create();
            return new QuantityTypeReadModel(quantityType, cacheMock.Object);
        }
    }
}
