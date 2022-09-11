using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Models;

public class PreparationStepMock : Mock<IPreparationStep>
{
    public PreparationStepMock(IPreparationStep step, MockBehavior behavior) : base(behavior)
    {
        SetupId(step.Id);
        SetupSortingIndex(step.SortingIndex);
    }

    public void SetupId(PreparationStepId id)
    {
        Setup(m => m.Id).Returns(id);
    }

    public void SetupSortingIndex(int sortingIndex)
    {
        Setup(m => m.SortingIndex).Returns(sortingIndex);
    }

    public void SetupModify(PreparationStepModification modification, IPreparationStep returnValue)
    {
        Setup(m => m.Modify(modification)).Returns(returnValue);
    }

    public void VerifyModify(PreparationStepModification modification, Func<Times> times)
    {
        Verify(m => m.Modify(modification), times);
    }
}