using ProjectHermes.ShoppingList.Frontend.Redux.Shared.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
public record EditedPreparationStep(Guid Key, Guid Id, string Name, int SortingIndex) : ISortableItem;