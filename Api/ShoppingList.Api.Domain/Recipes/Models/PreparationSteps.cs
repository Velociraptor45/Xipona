using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

public class PreparationSteps : IEnumerable<IPreparationStep>
{
    private readonly IPreparationStepFactory _preparationStepFactory;
    private readonly IDictionary<PreparationStepId, IPreparationStep> _steps;

    public PreparationSteps(IEnumerable<IPreparationStep> steps, IPreparationStepFactory preparationStepFactory)
    {
        _preparationStepFactory = preparationStepFactory;
        _steps = steps.ToDictionary(s => s.Id);

        ValidateSortingIndexes();
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

    public void ModifyMany(IEnumerable<PreparationStepModification> modifications)
    {
        var modificationsList = modifications.ToList();

        var preparationStepsToModify = modificationsList.Where(s => s.Id.HasValue)
            .ToDictionary(modification => modification.Id!.Value);
        var preparationStepsToCreate = modificationsList.Where(s => !s.Id.HasValue);
        var preparationStepIdsToDelete = _steps.Keys.Where(id => !preparationStepsToModify.ContainsKey(id));
        var newPreparationSteps = new List<IPreparationStep>();
        foreach (var modification in preparationStepsToCreate)
        {
            var newPreparationStep = _preparationStepFactory.CreateNew(modification.Instruction, modification.SortingIndex);
            newPreparationSteps.Add(newPreparationStep);
        }

        foreach (var preparationStepId in preparationStepIdsToDelete)
        {
            Remove(preparationStepId);
        }

        foreach (var preparationStep in preparationStepsToModify.Values)
        {
            Modify(preparationStep);
        }

        AddMany(newPreparationSteps);
        ValidateSortingIndexes();
    }

    private void Modify(PreparationStepModification modification)
    {
        if (!modification.Id.HasValue)
            throw new ArgumentException("Id mustn't be null.");

        if (!_steps.TryGetValue(modification.Id.Value, out var preparationStep))
        {
            throw new DomainException(new PreparationStepNotFoundReason(modification.Id.Value));
        }

        var modifiedType = preparationStep.Modify(modification);
        _steps[modifiedType.Id] = modifiedType;
    }

    private void Remove(PreparationStepId id)
    {
        _steps.Remove(id);
    }

    private void AddMany(IEnumerable<IPreparationStep> types)
    {
        foreach (var type in types)
        {
            Add(type);
        }
    }

    private void Add(IPreparationStep preparationStep)
    {
        _steps.Add(preparationStep.Id, preparationStep);
    }

    private void ValidateSortingIndexes()
    {
        var sortingIndexes = _steps.Values.Select(s => s.SortingIndex).ToLookup(si => si);
        foreach (var sortingIndex in sortingIndexes)
        {
            if (sortingIndex.Count() > 1)
                throw new DomainException(new DuplicatedSortingIndexReason(sortingIndex.Key));
        }
    }
}