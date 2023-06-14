namespace ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States;
public record ProcessingErrors(bool IsDebug, bool HasErrors, IReadOnlyCollection<string> Stack);