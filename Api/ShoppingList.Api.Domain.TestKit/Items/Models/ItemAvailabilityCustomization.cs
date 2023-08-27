using AutoFixture.Kernel;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models;

public class ItemAvailabilityCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customizations.Add(new ItemAvailabilityBuilder());
        fixture.Customizations.Add(new ItemAvailabilityEnumeratorBuilder());
    }

    public class ItemAvailabilityBuilder : ISpecimenBuilder
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
            if (expectedType == seededRequest?.Request)
                return true;

            var multipleRequest = request as MultipleRequest;
            if (expectedType == multipleRequest?.Request)
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

    public class ItemAvailabilityEnumeratorBuilder : ISpecimenBuilder
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
            if (request == type)
                return true;

            if ((request as SeededRequest)?.Request == type)
                return true;

            return false;
        }

        private IEnumerator<ItemAvailability> CreateInstance(ISpecimenContext context)
        {
            return context.CreateMany<ItemAvailability>(3).GetEnumerator();
        }
    }
}