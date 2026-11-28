using RestEase;

namespace Xipona.Frontend.Redux.Shared.Actions;
public record DisplayApiExceptionNotificationAction(string Title, ApiException Exception);