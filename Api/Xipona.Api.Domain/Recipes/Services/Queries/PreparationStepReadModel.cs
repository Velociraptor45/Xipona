using ProjectHermes.Xipona.Api.Domain.Recipes.Models;

namespace ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries;
public record PreparationStepReadModel(PreparationStepId Id, PreparationStepInstruction Instruction, int SortingIndex);