namespace ProjectHermes.Xipona.Api.Domain.Accounts.Models.Factories;

public class UserFactory : IUserFactory
{
    private readonly TimeProvider _timeProvider;

    public UserFactory(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    public IUser Create(UserId id, DateTimeOffset createAt)
    {
        return new User(id, createAt);
    }

    public IUser CreateNew(UserId id)
    {
        return new User(id, _timeProvider.GetUtcNow());
    }
}
