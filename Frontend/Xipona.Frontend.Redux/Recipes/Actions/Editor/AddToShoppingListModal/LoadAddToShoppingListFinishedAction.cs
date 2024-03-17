using ProjectHermes.Xipona.Frontend.Redux.Recipes.States;

namespace ProjectHermes.Xipona.Frontend.Redux.Recipes.Actions.Editor.AddToShoppingListModal;
public record LoadAddToShoppingListFinishedAction(IReadOnlyCollection<AddToShoppingListItem> ItemsForOneServing);