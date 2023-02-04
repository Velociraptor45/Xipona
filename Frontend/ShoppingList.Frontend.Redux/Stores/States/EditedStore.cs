namespace ProjectHermes.ShoppingList.Frontend.Redux.Stores.States;
public record EditedStore(Guid Id, string Name, IReadOnlyCollection<EditedSection> Sections);