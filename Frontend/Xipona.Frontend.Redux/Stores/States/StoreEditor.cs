namespace ProjectHermes.Xipona.Frontend.Redux.Stores.States;
public record StoreEditor(EditedStore? Store, bool IsSaving, bool IsDeleting, bool IsShowingDeletionNotice,
    EditorValidationResult ValidationResult);