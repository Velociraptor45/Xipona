using AutoFixture;
using AutoFixture.Kernel;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States.Comparer;

namespace ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Recipes.States;

public class EditedPreparationStepsCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customizations.Add(new EditedPreparationStepsSpecimenBuilder());
    }

    private class EditedPreparationStepsSpecimenBuilder : ISpecimenBuilder
    {
        private readonly Type _expectedType = typeof(SortedSet<EditedPreparationStep>);

        public object Create(object request, ISpecimenContext context)
        {
            if (!MatchesType(request))
                return new NoSpecimen();

            return CreateInstance(context);
        }

        private bool MatchesType(object request)
        {
            if (request is SeededRequest seededRequest
                && seededRequest.Request is Type requestType
                && requestType == _expectedType)
            {
                return true;
            }

            var t = request as Type;
            return _expectedType == t;
        }

        private static SortedSet<EditedPreparationStep> CreateInstance(ISpecimenContext context)
        {
            var ingredients = context.CreateMany<EditedPreparationStep>().ToList();
            return new SortedSet<EditedPreparationStep>(ingredients, new SortingIndexComparer());
        }
    }
}