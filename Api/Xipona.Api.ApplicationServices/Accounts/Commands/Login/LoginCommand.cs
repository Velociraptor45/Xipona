using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.Accounts.Models;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Accounts.Commands.Login;

public record LoginCommand(UserId UserId) : ICommand<IUser>;
