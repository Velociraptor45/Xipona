namespace ProjectHermes.Xipona.Frontend.Redux.Shared.States;

public record Settings(
    GeneralSettings? GeneralSettings,
    UserSettings? UserSettings,
    SettingsEditor? Editor,
    bool SettingsOpen,
    bool IsSaving);