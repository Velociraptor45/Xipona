using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.Common.Models;

namespace Xipona.Api.ApplicationServices.Users.Commands.UpdateGeneralSettings;

public record UpdateGeneralSettingsCommand(Currency Currency) : ICommand<bool>;
