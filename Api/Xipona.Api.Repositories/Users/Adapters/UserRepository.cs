using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Core.DomainEventHandlers;
using ProjectHermes.Xipona.Api.Domain.Accounts.Models;
using ProjectHermes.Xipona.Api.Domain.Accounts.Ports;
using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.Common.Models;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Repositories.Users.Contexts;
using User = ProjectHermes.Xipona.Api.Repositories.Users.Entities.User;

namespace ProjectHermes.Xipona.Api.Repositories.Users.Adapters;

public class UserRepository : IUserRepository
{
    private readonly UserContext _dbContext;
    private readonly IToDomainConverter<User, IUser> _toDomainConverter;
    private readonly IToContractConverter<IUser, User> _toContractConverter;
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private readonly ILogger<UserRepository> _logger;
    private readonly CancellationToken _cancellationToken;

    public UserRepository(UserContext dbContext,
        IToDomainConverter<User, IUser> toDomainConverter,
        IToContractConverter<IUser, User> toContractConverter,
        IDomainEventDispatcher domainEventDispatcher,
        ILogger<UserRepository> logger,
        CancellationToken cancellationToken)
    {
        _dbContext = dbContext;
        _toDomainConverter = toDomainConverter;
        _toContractConverter = toContractConverter;
        _domainEventDispatcher = domainEventDispatcher;
        _logger = logger;
        _cancellationToken = cancellationToken;
    }

    public async Task<bool> ExistsAsync(UserId id)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .AnyAsync(i => i.Id == id, _cancellationToken);
    }

    public async Task<IUser?> FindByAsync(UserId id)
    {
        var entity = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == id, _cancellationToken);
        if (entity is null)
            return null;

        return _toDomainConverter.ToDomain(entity);
    }

    public async Task<IUser> StoreAsync(IUser model)
    {
        var convertedEntity = _toContractConverter.ToContract(model);
        var existingEntity = await FindTrackedEntityById(model.Id, _cancellationToken);

        if (existingEntity is null)
        {
            _dbContext.Entry(convertedEntity).State = EntityState.Added;
        }
        else
        {
            var existingRowVersion = existingEntity.RowVersion;
            _dbContext.Entry(existingEntity).CurrentValues.SetValues(convertedEntity);
            _dbContext.Entry(existingEntity).State = EntityState.Modified;
            _dbContext.Entry(existingEntity).Property(r => r.RowVersion).OriginalValue = existingRowVersion;
        }

        try
        {
            await _dbContext.SaveChangesAsync(_cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogInformation(ex,
                "Saving user '{UserId}' failed due to concurrency violation", model.Id.Value);
            throw new DomainException(new ModelOutOfDateReason());
        }

        await ((AggregateRoot)model).DispatchDomainEvents(_domainEventDispatcher);

        return _toDomainConverter.ToDomain(convertedEntity);
    }

    private async Task<User?> FindTrackedEntityById(UserId id, CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
    }
}
