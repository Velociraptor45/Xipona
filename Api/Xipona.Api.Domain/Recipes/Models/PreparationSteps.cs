using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models.Factories;
using ProjectHermes.Xipona.Api.Domain.Recipes.Reasons;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Modifications;
using ProjectHermes.Xipona.Api.Domain.Shared.Models;

namespace ProjectHermes.Xipona.Api.Domain.Recipes.Models;

public class PreparationSteps : IEnumerable<IPreparationStep>, ISortableCollection<IPreparationStep>
{
    private readonly IPreparationStepFactory _preparationStepFactory;
    private readonly IDictionary<PreparationStepId, IPreparationStep> _steps;

    public PreparationSteps(IEnumerable<IPreparationStep> steps, IPreparationStepFactory preparationStepFactory)
    {
        _preparationStepFactory = preparationStepFactory;
        _steps = steps.ToDictionary(s => s.Id);

        AsSortableCollection.ValidateSortingIndexes(_steps.Values);
    }

    private ISortableCollection<IPreparationStep> AsSortableCollection => this;

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
        AsSortableCollection.ValidateSortingIndexes(_steps.Values);
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
}