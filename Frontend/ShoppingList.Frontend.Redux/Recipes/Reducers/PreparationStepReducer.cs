using Fluxor;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor.PreparationSteps;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
using ShoppingList.Frontend.Redux.Recipes.States;
using ShoppingList.Frontend.Redux.ShoppingList.States.Comparer;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Reducers;

public static class PreparationStepReducer
{
    [ReducerMethod]
    public static RecipeState OnIncrementPreparationStep(RecipeState state, IncrementPreparationStepAction action)
    {
        var steps = state.Editor.Recipe!.PreparationSteps.ToList();

        int sectionIndex = steps.IndexOf(action.PreparationStep);
        if (sectionIndex == -1 || sectionIndex >= steps.Count - 1)
            return state;

        var incremented = steps[sectionIndex];
        var decremented = steps[sectionIndex + 1];

        steps[sectionIndex + 1] = decremented with { SortingIndex = incremented.SortingIndex };
        steps[sectionIndex] = incremented with { SortingIndex = decremented.SortingIndex };

        return state with
        {
            Editor = state.Editor with
            {
                Recipe = state.Editor.Recipe with
                {
                    PreparationSteps = new SortedSet<EditedPreparationStep>(steps, new SortingIndexComparer())
                }
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnDecrementPreparationStep(RecipeState state, DecrementPreparationStepAction action)
    {
        var steps = state.Editor.Recipe!.PreparationSteps.ToList();

        int sectionIndex = steps.IndexOf(action.PreparationStep);
        if (sectionIndex is -1 or <= 0)
            return state;

        var decremented = steps[sectionIndex];
        var incremented = steps[sectionIndex - 1];

        steps[sectionIndex - 1] = incremented with { SortingIndex = decremented.SortingIndex };
        steps[sectionIndex] = decremented with { SortingIndex = incremented.SortingIndex };

        return state with
        {
            Editor = state.Editor with
            {
                Recipe = state.Editor.Recipe with
                {
                    PreparationSteps = new SortedSet<EditedPreparationStep>(steps, new SortingIndexComparer())
                }
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnPreparationStepRemoved(RecipeState state, PreparationStepRemovedAction action)
    {
        var steps = state.Editor.Recipe!.PreparationSteps.ToList();
        steps.Remove(action.PreparationStep);

        return state with
        {
            Editor = state.Editor with
            {
                Recipe = state.Editor.Recipe with
                {
                    PreparationSteps = new SortedSet<EditedPreparationStep>(steps, new SortingIndexComparer())
                }
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnPreparationStepTextChanged(RecipeState state, PreparationStepTextChangedAction action)
    {
        var steps = state.Editor.Recipe!.PreparationSteps.ToList();
        var step = steps.FirstOrDefault(s => s.Key == action.PreparationStepKey);
        if (step is null)
            return state;

        var stepIndex = steps.IndexOf(step);
        steps[stepIndex] = step with { Name = action.Text };

        return state with
        {
            Editor = state.Editor with
            {
                Recipe = state.Editor.Recipe with
                {
                    PreparationSteps = new SortedSet<EditedPreparationStep>(steps, new SortingIndexComparer())
                }
            }
        };
    }

    [ReducerMethod(typeof(PreparationStepAddedAction))]
    public static RecipeState OnPreparationStepAdded(RecipeState state)
    {
        var steps = state.Editor.Recipe!.PreparationSteps.ToList();
        var maxSortingIndex = steps.Any() ? steps.Max(s => s.SortingIndex) : 0;
        steps.Add(new EditedPreparationStep(Guid.NewGuid(), Guid.Empty, "", maxSortingIndex + 1));

        return state with
        {
            Editor = state.Editor with
            {
                Recipe = state.Editor.Recipe with
                {
                    PreparationSteps = new SortedSet<EditedPreparationStep>(steps, new SortingIndexComparer())
                }
            }
        };
    }
}