using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.Users.Models;

namespace Xipona.Api.ApplicationServices.Users.Commands.Login;

public record LoginCommand(UserId UserId) : ICommand<IUser>;
