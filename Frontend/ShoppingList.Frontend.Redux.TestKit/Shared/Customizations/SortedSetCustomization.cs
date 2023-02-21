using AutoFixture;
using AutoFixture.Kernel;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States.Comparer;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;

namespace ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Shared.Customizations;

public class SortedSetCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customizations.Add(new ShoppingListSectionSortedSetSpecimenBuilder());
    }

    private class ShoppingListSectionSortedSetSpecimenBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (!MatchesType(request))
                return new NoSpecimen();

            return CreateInstance(context);
        }

        private static bool MatchesType(object request)
        {
            var expectedType = typeof(SortedSet<ShoppingListSection>);
            if (request is SeededRequest seededRequest)
            {
                var requestType = seededRequest.Request as Type;
                return expectedType == requestType;
            }

            var t = request as Type;
            return expectedType == t;
        }

        private static SortedSet<ShoppingListSection> CreateInstance(ISpecimenContext context)
        {
            var sections = new DomainTestBuilderBase<ShoppingListSection>().CreateMany(3).ToList(); // todo replace
            for (int i = 0; i < sections.Count; i++)
            {
                sections[i] = sections[i] with
                {
                    SortingIndex = i
                };
            }

            return new SortedSet<ShoppingListSection>(sections, new SortingIndexComparer());
        }
    }
}