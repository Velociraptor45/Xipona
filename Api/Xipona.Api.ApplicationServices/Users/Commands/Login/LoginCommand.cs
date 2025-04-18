using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.Users.Models;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Users.Commands.Login;

public record LoginCommand(UserId UserId) : ICommand<IUser>;
