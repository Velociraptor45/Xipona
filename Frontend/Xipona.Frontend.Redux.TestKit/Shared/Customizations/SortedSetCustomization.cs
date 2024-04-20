using AutoFixture;
using AutoFixture.Kernel;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.States;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States.Comparer;
using ProjectHermes.Xipona.Frontend.Redux.Stores.States;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;

namespace ProjectHermes.Xipona.Frontend.Redux.TestKit.Shared.Customizations;

public class SortedSetCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customizations.Add(new ShoppingListSectionSortedSetSpecimenBuilder());
        fixture.Customizations.Add(new EditedPreparationStepSortedSetSpecimenBuilder());
        fixture.Customizations.Add(new EditedSectionSortedSetSpecimenBuilder());
    }

    private class ShoppingListSectionSortedSetSpecimenBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (!MatchesType(request))
                return new NoSpecimen();

            return CreateInstance();
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

        private static SortedSet<ShoppingListSection> CreateInstance()
        {
            var sections = new DomainTestBuilderBase<ShoppingListSection>().CreateMany(3).ToList();
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

    private class EditedPreparationStepSortedSetSpecimenBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (!MatchesType(request))
                return new NoSpecimen();

            return CreateInstance();
        }

        private static bool MatchesType(object request)
        {
            var expectedType = typeof(SortedSet<EditedPreparationStep>);
            if (request is SeededRequest seededRequest)
            {
                var requestType = seededRequest.Request as Type;
                return expectedType == requestType;
            }

            var t = request as Type;
            return expectedType == t;
        }

        private static SortedSet<EditedPreparationStep> CreateInstance()
        {
            var sections = new DomainTestBuilderBase<EditedPreparationStep>().CreateMany(3).ToList();
            for (int i = 0; i < sections.Count; i++)
            {
                sections[i] = sections[i] with
                {
                    SortingIndex = i
                };
            }

            return new SortedSet<EditedPreparationStep>(sections, new SortingIndexComparer());
        }
    }

    private class EditedSectionSortedSetSpecimenBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (!MatchesType(request))
                return new NoSpecimen();

            return CreateInstance();
        }

        private static bool MatchesType(object request)
        {
            var expectedType = typeof(SortedSet<EditedSection>);
            if (request is SeededRequest seededRequest)
            {
                var requestType = seededRequest.Request as Type;
                return expectedType == requestType;
            }

            var t = request as Type;
            return expectedType == t;
        }

        private static SortedSet<EditedSection> CreateInstance()
        {
            var sections = new DomainTestBuilderBase<EditedSection>().CreateMany(3).ToList();
            for (int i = 0; i < sections.Count; i++)
            {
                sections[i] = sections[i] with
                {
                    IsDefaultSection = i == 0,
                    SortingIndex = i
                };
            }

            return new SortedSet<EditedSection>(sections, new SortingIndexComparer());
        }
    }
}