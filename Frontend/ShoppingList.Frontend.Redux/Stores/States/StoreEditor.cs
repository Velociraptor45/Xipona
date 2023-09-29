namespace ProjectHermes.ShoppingList.Frontend.Redux.Stores.States;
public record StoreEditor(EditedStore? Store, bool IsSaving, bool IsDeleting, bool IsShowingDeletionNotice);