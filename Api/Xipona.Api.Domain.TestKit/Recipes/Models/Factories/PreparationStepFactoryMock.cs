using ProjectHermes.Xipona.Api.Domain.Recipes.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models.Factories;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Creations;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.Recipes.Models.Factories;

public class PreparationStepFactoryMock : Mock<IPreparationStepFactory>
{
    public PreparationStepFactoryMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupCreateNew(PreparationStepCreation creation, IPreparationStep returnValue)
    {
        Setup(m => m.CreateNew(creation))
            .Returns(returnValue);
    }

    public void SetupCreateNew(PreparationStepInstruction instruction, int sortingIndex, IPreparationStep returnValue)
    {
        Setup(m => m.CreateNew(instruction, sortingIndex))
            .Returns(returnValue);
    }

    public void SetupCreate(PreparationStepId id, PreparationStepInstruction instruction, int sortingIndex,
        IPreparationStep returnValue)
    {
        Setup(m => m.Create(id, instruction, sortingIndex))
            .Returns(returnValue);
    }
}