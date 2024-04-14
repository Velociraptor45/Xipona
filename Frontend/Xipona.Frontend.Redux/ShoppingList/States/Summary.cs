namespace ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;
public record Summary(bool IsOpen, bool IsSaving, DateTime FinishedAt,
    bool IsEditingFinishedAt);