using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using Polly;
using Polly.Retry;
using Xipona.Api.Core.Converter;
using Xipona.Api.Core.DomainEventHandlers;
using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.ItemCategories.Ports;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Items.Ports;
using Xipona.Api.Domain.Manufacturers.Models;
using Xipona.Api.Domain.Manufacturers.Ports;
using Xipona.Api.Domain.Recipes.Models;
using Xipona.Api.Domain.Recipes.Ports;
using Xipona.Api.Domain.Recipes.Services.Queries;
using Xipona.Api.Domain.RecipeTags.Models;
using Xipona.Api.Domain.RecipeTags.Ports;
using Xipona.Api.Domain.ShoppingLists.Models;
using Xipona.Api.Domain.ShoppingLists.Ports;
using Xipona.Api.Domain.Stores.Models;
using Xipona.Api.Domain.Stores.Ports;
using Xipona.Api.Domain.Users.Models;
using Xipona.Api.Domain.Users.Ports;
using Xipona.Api.Repositories.Common.Transactions;
using Xipona.Api.Repositories.ItemCategories.Adapters;
using Xipona.Api.Repositories.ItemCategories.Contexts;
using Xipona.Api.Repositories.Items.Adapters;
using Xipona.Api.Repositories.Items.Contexts;
using Xipona.Api.Repositories.Manufacturers.Adapters;
using Xipona.Api.Repositories.Manufacturers.Contexts;
using Xipona.Api.Repositories.Recipes.Adapters;
using Xipona.Api.Repositories.Recipes.Contexts;
using Xipona.Api.Repositories.RecipeTags.Adapters;
using Xipona.Api.Repositories.RecipeTags.Contexts;
using Xipona.Api.Repositories.ShoppingLists.Adapters;
using Xipona.Api.Repositories.ShoppingLists.Contexts;
using Xipona.Api.Repositories.Stores.Adapters;
using Xipona.Api.Repositories.Stores.Contexts;
using Xipona.Api.Repositories.Users.Adapters;
using Xipona.Api.Repositories.Users.Contexts;
using Xipona.Api.Secrets.Configs;
using System.Data.Common;
using GeneralSetting = Xipona.Api.Repositories.Users.Entities.GeneralSetting;
using Recipe = Xipona.Api.Repositories.Recipes.Entities.Recipe;
using RecipeTag = Xipona.Api.Repositories.RecipeTags.Entities.RecipeTag;
using User = Xipona.Api.Repositories.Users.Entities.User;

namespace Xipona.Api.Repositories;

public static class ServiceCollectionExtensions
{
    public static void AddRepositories(this IServiceCollection services, string? connectionString = null)
    {
        var retryOpt = new RetryStrategyOptions()
        {
            MaxRetryAttempts = 5,
            Delay = TimeSpan.FromSeconds(5),
            OnRetry = static args =>
            {
                Console.WriteLine($"Failed to open DB connection (Try no. {args.AttemptNumber}): {args.Outcome.Exception}");

                return default;
            }
        };
        var pipeline = new ResiliencePipelineBuilder().AddRetry(retryOpt).Build();

        services.AddScoped<DbConnection>(provider =>
        {
            if (connectionString is null)
            {
                var cs = provider.GetRequiredService<ConnectionStrings>();
                connectionString = cs.ShoppingDatabase;
            }

            return pipeline.Execute(() =>
            {
                var connection = new NpgsqlConnection(connectionString);
                connection.Open();
                return connection;
            });
        });

        services.AddScoped<IList<DbContext>>(serviceProvider => GetAllDbContextInstances(serviceProvider).ToList());

        services.AddDbContext<GeneralSettingContext>(SetDbConnection);
        services.AddDbContext<UserContext>(SetDbConnection);
        services.AddDbContext<ShoppingListContext>(SetDbConnection);
        services.AddDbContext<ItemCategoryContext>(SetDbConnection);
        services.AddDbContext<ManufacturerContext>(SetDbConnection);
        services.AddDbContext<ItemContext>(SetDbConnection);
        services.AddDbContext<StoreContext>(SetDbConnection);
        services.AddDbContext<RecipeContext>(SetDbConnection);
        services.AddDbContext<RecipeTagContext>(SetDbConnection);

        services.AddTransient<Func<CancellationToken, IUserRepository>>(provider =>
        {
            return ct => new UserRepository(
                provider.GetRequiredService<UserContext>(),
                provider.GetRequiredService<IToDomainConverter<User, IUser>>(),
                provider.GetRequiredService<IToContractConverter<IUser, User>>(),
                provider.GetRequiredService<Func<CancellationToken, IDomainEventDispatcher>>()(ct),
                provider.GetRequiredService<ILogger<UserRepository>>(),
                ct);
        });

        services.AddTransient<Func<CancellationToken, IGeneralSettingRepository>>(provider =>
        {
            return ct => new GeneralSettingRepository(
                provider.GetRequiredService<GeneralSettingContext>(),
                provider.GetRequiredService<IToDomainConverter<GeneralSetting, IGeneralSetting>>(),
                provider.GetRequiredService<IToContractConverter<IGeneralSetting, GeneralSetting>>(),
                provider.GetRequiredService<ILogger<GeneralSettingRepository>>(),
                ct);
        });

        services.AddTransient<Func<CancellationToken, IShoppingListRepository>>(provider =>
        {
            return ct => new ShoppingListRepository(
                provider.GetRequiredService<ShoppingListContext>(),
                provider.GetRequiredService<IToDomainConverter<ShoppingLists.Entities.ShoppingList, IShoppingList>>(),
                provider.GetRequiredService<IToContractConverter<IShoppingList, ShoppingLists.Entities.ShoppingList>>(),
                provider.GetRequiredService<ILogger<ShoppingListRepository>>(),
                ct);
        });
        services.AddTransient<Func<CancellationToken, IItemRepository>>(provider =>
        {
            return ct => new ItemRepository(
                provider.GetRequiredService<ItemContext>(),
                provider.GetRequiredService<IToDomainConverter<Items.Entities.Item, IItem>>(),
                provider.GetRequiredService<IToContractConverter<IItem, Items.Entities.Item>>(),
                provider.GetRequiredService<Func<CancellationToken, IDomainEventDispatcher>>()(ct),
                provider.GetRequiredService<ILogger<ItemRepository>>(),
                ct);
        });
        services.AddTransient<Func<CancellationToken, IItemTypeReadRepository>>(provider =>
        {
            var dbContext = provider.GetRequiredService<ItemContext>();
            var converter = provider.GetRequiredService<IToDomainConverter<Items.Entities.ItemType, IItemType>>();
            return ct => new ItemTypeReadRepository(dbContext, converter, ct);
        });

        services.AddTransient<Func<CancellationToken, IItemCategoryRepository>>(provider =>
        {
            var context = provider.GetRequiredService<ItemCategoryContext>();
            var toDomainConverter = provider
                .GetRequiredService<IToDomainConverter<ItemCategories.Entities.ItemCategory, IItemCategory>>();
            var toEntityConverter = provider
                .GetRequiredService<IToContractConverter<IItemCategory, ItemCategories.Entities.ItemCategory>>();
            var dispatcher = provider.GetRequiredService<Func<CancellationToken, IDomainEventDispatcher>>();
            var logger = provider.GetRequiredService<ILogger<ItemCategoryRepository>>();
            return ct => new ItemCategoryRepository(context, toDomainConverter, toEntityConverter, dispatcher(ct), logger,
                ct);
        });
        services.AddTransient<Func<CancellationToken, IManufacturerRepository>>(provider =>
        {
            return ct => new ManufacturerRepository(
                provider.GetRequiredService<ManufacturerContext>(),
                provider.GetRequiredService<IToDomainConverter<Manufacturers.Entities.Manufacturer, IManufacturer>>(),
                provider.GetRequiredService<IToContractConverter<IManufacturer, Manufacturers.Entities.Manufacturer>>(),
                provider.GetRequiredService<ILogger<ManufacturerRepository>>(),
                ct);
        });
        services.AddTransient<Func<CancellationToken, IStoreRepository>>(provider =>
        {
            return ct => new StoreRepository(
                provider.GetRequiredService<StoreContext>(),
                provider.GetRequiredService<IToDomainConverter<Stores.Entities.Store, IStore>>(),
                provider.GetRequiredService<IToContractConverter<IStore, Stores.Entities.Store>>(),
                provider.GetRequiredService<Func<CancellationToken, IDomainEventDispatcher>>(),
                provider.GetRequiredService<ILogger<StoreRepository>>(),
                ct);
        });
        services.AddTransient<Func<CancellationToken, IRecipeRepository>>(provider =>
        {
            var context = provider.GetRequiredService<RecipeContext>();
            var searchResultToDomainConverter = provider.GetRequiredService<IToDomainConverter<Recipe, RecipeSearchResult>>();
            var toDomainConverter = provider.GetRequiredService<IToDomainConverter<Recipe, IRecipe>>();
            var toContractConverter = provider.GetRequiredService<IToContractConverter<IRecipe, Recipe>>();
            var logger = provider.GetRequiredService<ILogger<RecipeRepository>>();
            return cancellationToken =>
                new RecipeRepository(context, searchResultToDomainConverter, toDomainConverter, toContractConverter,
                    logger, cancellationToken);
        });

        services.AddTransient<Func<CancellationToken, IRecipeReadRepository>>(provider =>
        {
            return ct => new RecipeReadRepository(
                provider.GetRequiredService<RecipeContext>(),
                provider.GetRequiredService<IToDomainConverter<Recipe, SideDishReadModel>>(),
                ct);
        });

        services.AddTransient<Func<CancellationToken, IRecipeTagRepository>>(provider =>
        {
            var context = provider.GetRequiredService<RecipeTagContext>();
            var toDomainConverter = provider.GetRequiredService<IToDomainConverter<RecipeTag, IRecipeTag>>();
            var toContractConverter = provider.GetRequiredService<IToContractConverter<IRecipeTag, RecipeTag>>();
            var logger = provider.GetRequiredService<ILogger<RecipeTagRepository>>();
            return cancellationToken =>
                new RecipeTagRepository(context, toDomainConverter, toContractConverter, logger, cancellationToken);
        });
        services.AddScoped(_ => new SemaphoreSlim(1, 1));
        services.AddScoped<ITransactionGenerator, TransactionGenerator>();

        services.AddToDomainConverter();
        services.AddToContractConverter();
    }

    private static void SetDbConnection(IServiceProvider serviceProvider, DbContextOptionsBuilder options)
    {
        var connection = serviceProvider.GetService<DbConnection>()!;
        options.UseNpgsql(connection);
    }

    private static IEnumerable<DbContext> GetAllDbContextInstances(IServiceProvider serviceProvider)
    {
        var types = GetAllDbContextTypes();
        var instances = types.Select(serviceProvider.GetRequiredService);
        foreach (var instance in instances)
        {
            yield return (DbContext)instance;
        }
    }

    private static IEnumerable<Type> GetAllDbContextTypes()
    {
        // The order of the types is important, because the migrations are applied in the same order
        // and some of them depend on others
        yield return typeof(GeneralSettingContext);
        yield return typeof(UserContext);
        yield return typeof(ManufacturerContext);
        yield return typeof(ItemCategoryContext);
        yield return typeof(StoreContext);
        yield return typeof(ItemContext);
        yield return typeof(ShoppingListContext);
        yield return typeof(RecipeTagContext);
        yield return typeof(RecipeContext);
    }
}