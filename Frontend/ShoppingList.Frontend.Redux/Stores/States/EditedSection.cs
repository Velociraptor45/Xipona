using ProjectHermes.ShoppingList.Frontend.Redux.Shared.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Stores.States;

public record EditedSection(Guid Key, Guid Id, string Name, bool IsDefaultSection, int SortingIndex) : ISortableItem;