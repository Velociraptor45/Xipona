using Xipona.Frontend.Redux.Recipes.States;

namespace Xipona.Frontend.Redux.Recipes.Actions.Editor.AddToShoppingListModal;
public record LoadAddToShoppingListFinishedAction(IReadOnlyCollection<AddToShoppingListItem> ItemsForOneServing);