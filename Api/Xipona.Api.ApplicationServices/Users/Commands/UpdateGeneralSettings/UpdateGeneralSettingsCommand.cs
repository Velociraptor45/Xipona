using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.Common.Models;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Users.Commands.UpdateGeneralSettings;

public record UpdateGeneralSettingsCommand(Currency Currency) : ICommand<bool>;
