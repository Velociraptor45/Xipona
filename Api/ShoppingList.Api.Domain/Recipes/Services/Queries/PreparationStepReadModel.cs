using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;
public record PreparationStepReadModel(PreparationStepId Id, PreparationStepInstruction Instruction, int SortingIndex);