using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Reasons;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

public class PreparationSteps : IEnumerable<IPreparationStep>
{
    private readonly IDictionary<PreparationStepId, IPreparationStep> _steps;

    public PreparationSteps(IEnumerable<IPreparationStep> steps)
    {
        var stepsList = steps.ToList();

        var set = new HashSet<int>();
        foreach (var idx in stepsList.Select(s => s.SortingIndex))
        {
            if (set.Contains(idx))
                throw new DomainException(new DuplicatedSortingIndexReason(idx));
            set.Add(idx);
        }

        _steps = stepsList.ToDictionary(s => s.Id);
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