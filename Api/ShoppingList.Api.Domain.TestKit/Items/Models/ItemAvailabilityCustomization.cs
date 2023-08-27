using AutoFixture.Kernel;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models;

public class ItemAvailabilityCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customizations.Add(new ItemAvailabilitySpecimenBuilder());
        fixture.Customizations.Add(new ItemAvailabilityEnumeratorSpecimenBuilder());
    }

    public class ItemAvailabilitySpecimenBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (!MatchesType(request))
                return new NoSpecimen();

            return CreateInstance(context);
        }

        private static bool MatchesType(object request)
        {
            var expectedType = typeof(ItemAvailability);
            var type = request as Type;
            if (expectedType == type)
                return true;

            var seededRequest = request as SeededRequest;
            if (expectedType == seededRequest?.Request as Type)
                return true;

            var multipleRequest = request as MultipleRequest;
            if (expectedType == multipleRequest?.Request as Type)
                return true;

            return false;
        }

        private ItemAvailability CreateInstance(ISpecimenContext context)
        {
            return new ItemAvailability(
                StoreId.New,
                (Price)context.Resolve(typeof(Price)),
                SectionId.New);
        }
    }

    public class ItemAvailabilityEnumeratorSpecimenBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (!MatchesType(request))
                return new NoSpecimen();

            return CreateInstance(context);
        }

        private static bool MatchesType(object request)
        {
            var type = typeof(IEnumerator<ItemAvailability>);
            if (request as Type == type)
                return true;

            if ((request as SeededRequest)?.Request as Type == type)
                return true;

            return false;
        }

        private IEnumerator<ItemAvailability> CreateInstance(ISpecimenContext context)
        {
            return context.CreateMany<ItemAvailability>(3).GetEnumerator();
        }
    }
}