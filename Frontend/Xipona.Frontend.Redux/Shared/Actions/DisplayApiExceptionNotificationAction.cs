using RestEase;

namespace ProjectHermes.Xipona.Frontend.Redux.Shared.Actions;
public record DisplayApiExceptionNotificationAction(string Title, ApiException Exception);