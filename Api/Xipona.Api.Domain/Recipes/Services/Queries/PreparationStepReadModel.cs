using Xipona.Api.Domain.Recipes.Models;

namespace Xipona.Api.Domain.Recipes.Services.Queries;
public record PreparationStepReadModel(PreparationStepId Id, PreparationStepInstruction Instruction, int SortingIndex);