using ProjectHermes.ShoppingList.Frontend.Models.Shared;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
public record EditedPreparationStep(Guid Key, Guid Id, string Name, int SortingIndex) : ISortableItem;