using ProjectHermes.Xipona.Frontend.Redux.Shared.States;

namespace ProjectHermes.Xipona.Frontend.Redux.Recipes.States;
public record EditedPreparationStep(Guid Key, Guid Id, string Name, int SortingIndex) : ISortableItem;