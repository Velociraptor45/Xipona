namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

public class PreparationSteps : IEnumerable<IPreparationStep>
{
    private readonly IDictionary<PreparationStepId, IPreparationStep> _steps;

    public PreparationSteps(IEnumerable<IPreparationStep> steps)
    {
        // todo sorting order validation

        _steps = steps.ToDictionary(s => s.Id);
    }

    public IReadOnlyCollection<IPreparationStep> AsReadOnly()
    {
        return _steps.Values.ToList().AsReadOnly();
    }

    public IEnumerator<IPreparationStep> GetEnumerator()
    {
        return _steps.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}