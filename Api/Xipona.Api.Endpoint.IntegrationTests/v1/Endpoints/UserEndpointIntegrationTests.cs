using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.Contracts.Common;
using ProjectHermes.Xipona.Api.Contracts.Items.Queries.SearchItemsForShoppingLists;
using ProjectHermes.Xipona.Api.Contracts.Users.Commands.AllCurrencies;
using ProjectHermes.Xipona.Api.Contracts.Users.Commands.Login;
using ProjectHermes.Xipona.Api.Contracts.Users.Commands.UpdateGeneralSettings;
using ProjectHermes.Xipona.Api.Core.Constants;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Common.Models;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Searches;
using ProjectHermes.Xipona.Api.Domain.TestKit.Shared;
using ProjectHermes.Xipona.Api.Domain.Users.Models;
using ProjectHermes.Xipona.Api.Domain.Users.Services.Queries;
using ProjectHermes.Xipona.Api.Endpoint.v1.Endpoints;
using ProjectHermes.Xipona.Api.Repositories.ItemCategories.Contexts;
using ProjectHermes.Xipona.Api.Repositories.ItemCategories.Entities;
using ProjectHermes.Xipona.Api.Repositories.Items.Contexts;
using ProjectHermes.Xipona.Api.Repositories.Items.Entities;
using ProjectHermes.Xipona.Api.Repositories.Manufacturers.Contexts;
using ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Contexts;
using ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Entities;
using ProjectHermes.Xipona.Api.Repositories.Stores.Contexts;
using ProjectHermes.Xipona.Api.Repositories.Stores.Entities;
using ProjectHermes.Xipona.Api.Repositories.TestKit.ItemCategories.Entities;
using ProjectHermes.Xipona.Api.Repositories.TestKit.Items.Entities;
using ProjectHermes.Xipona.Api.Repositories.TestKit.ShoppingLists.Entities;
using ProjectHermes.Xipona.Api.Repositories.TestKit.Stores.Entities;
using ProjectHermes.Xipona.Api.Repositories.Users.Contexts;
using ProjectHermes.Xipona.Api.TestTools.AutoFixture;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using ProjectHermes.Xipona.Api.WebApp.Auth;
using System;
using System.IdentityModel.Tokens.Jwt;
using Xunit;
using GeneralSetting = ProjectHermes.Xipona.Api.Repositories.Users.Entities.GeneralSetting;
using User = ProjectHermes.Xipona.Api.Repositories.Users.Entities.User;

namespace ProjectHermes.Xipona.Api.Endpoint.IntegrationTests.v1.Endpoints;

public class UserEndpointIntegrationTests
{
    public sealed class Login : IAssemblyFixture<DockerFixture>
    {
        private readonly LoginFixture _fixture;
        public Login(DockerFixture dockerFixture)
        {
            _fixture = new LoginFixture(dockerFixture);
        }

        [Fact]
        public async Task Login_WithValidAccessToken_ShouldCreateUser()
        {
            // Arrange
            _fixture.SetupExpectedUser();
            _fixture.SetupExpectedResult();
            await _fixture.PrepareDatabaseAsync();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedUser);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Ok<UserInfoContract>>();

            var okResult = (Ok<UserInfoContract>)result;
            okResult.Value.Should().BeEquivalentTo(_fixture.ExpectedResult);

            using var assertionServiceScope = _fixture.CreateServiceScope();

            var users = await _fixture.LoadAllUsersAsync(assertionServiceScope);
            users.Should().HaveCount(1);
            var user = users[0];
            user.Should().BeEquivalentTo(_fixture.ExpectedUser,
                opt => opt.ExcludeRowVersion().WithCreatedAtPrecision(TimeSpan.FromSeconds(30)));
        }

        private sealed class LoginFixture : UserEndpointFixture
        {
            public LoginFixture(DockerFixture dockerFixture) : base(dockerFixture)
            {
            }

            public User? ExpectedUser { get; private set; }
            public UserInfoContract? ExpectedResult { get; private set; }

            public async Task<IResult> ActAsync()
            {
                var scope = CreateServiceScope();
                var ctx = new DefaultHttpContext();
                ctx.Request.Headers.Authorization = "Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIwMDVjNDJkYy0zNTI5LTQwODYtOTY4OS1kMjNiNjNkYTYzY2EiLCJnaXZlbl9uYW1lIjoiSm9obi1vaCBEZWVyIn0.CEBWi9GOoMZKC0Lu_qSXbCGbbv7UlCSeR_ttPAC1N-YVd2RXnHVAKe0BLhvgRdB8jMY0LXse2LrODcKtC0eLvFjMCg6TWGSuS1cM-QP-yrHRPpzvNNRZojkSSfeTQcmo475dSCcNpqMtJhvYj0d6bAuunFv0vm5yL3tWRNgtDKPUvsx8DkbTcq1-r6F8K7LCxUWjWO6mj-Vva0AbkJQIgO32e3j7Ny2ArykTpvFG00-5uVxc9po7OMIEw7ld23PFwqkrAD9Z9plxXK9GTTNHj-4sh0P-MqrONrXOlER5Bqx6lzis0freHPV_NspXXoMGK599S9eFG2CPsU7pR-SXjg";

                return await UserEndpoints.Login(
                    ctx,
                    new JwtSecurityTokenHandler(),
                    scope.ServiceProvider.GetRequiredService<ICommandDispatcher>(),
                    scope.ServiceProvider.GetRequiredService<IToContractConverter<IReason, ErrorContract>>(),
                    new AuthenticationOptions
                    {
                        NameClaimType = "given_name"
                    },
                    default);
            }

            public async Task PrepareDatabaseAsync()
            {
                await ApplyMigrationsAsync(ArrangeScope);
            }

            public void SetupExpectedUser()
            {
                ExpectedUser = new User
                {
                    Id = Guid.Parse("005c42dc-3529-4086-9689-d23b63da63ca"),
                    CreatedAt = DateTimeOffset.UtcNow,
                };
            }

            public void SetupExpectedResult()
            {
                ExpectedResult = new UserInfoContract
                {
                    DisplayName = "John-oh Deer"
                };
            }

        }
    }

    public sealed class UpdateGeneralSettings(DockerFixture dockerFixture) : IAssemblyFixture<DockerFixture>
    {
        private readonly UpdateGeneralSettingsFixture _fixture = new(dockerFixture);

        [Fact]
        public async Task UpdateGeneralSettings_WithValidData_ShouldUpdateSettings()
        {
            // Arrange
            await _fixture.PrepareDatabaseAsync();
            _fixture.SetupContract();
            _fixture.SetupExpectedResult();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            result.Should().BeOfType<NoContent>();

            var assertionScope = _fixture.CreateServiceScope();
            var generalSettings = await _fixture.LoadAllGeneralSettingsAsync(assertionScope);
            generalSettings.Should().HaveCount(1);
            var generalSetting = generalSettings[0];
            generalSetting.Should().BeEquivalentTo(_fixture.ExpectedResult, opt => opt.ExcludeRowVersion());
        }

        [Fact]
        public async Task UpdateGeneralSettings_WithValidData_ShouldChangeItemSearchCurrency()
        {
            // Arrange
            _fixture.SetupItem();
            await _fixture.PrepareDatabaseWithItemAsync();
            _fixture.SetupContract();
            _fixture.SetupExpectedResult();
            _fixture.SetupMemoryCache();

            var searchResultBefore = await _fixture.GetSearchResultAsync();
            searchResultBefore.Should().NotBeNull();
            searchResultBefore.Should().BeOfType<Ok<List<SearchItemForShoppingListResultContract>>>();
            var okResultBefore = (Ok<List<SearchItemForShoppingListResultContract>>)searchResultBefore;
            var itemBefore = okResultBefore.Value![0];
            itemBefore.PriceLabel.Should().Be("¥");

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            await _fixture.ActAsync();

            // Assert
            var searchResultAfter = await _fixture.GetSearchResultAsync();
            searchResultAfter.Should().NotBeNull();
            searchResultAfter.Should().BeOfType<Ok<List<SearchItemForShoppingListResultContract>>>();
            var okResultAfter = (Ok<List<SearchItemForShoppingListResultContract>>)searchResultAfter;
            var itemAfter = okResultAfter.Value![0];
            itemAfter.PriceLabel.Should().Be("$");
        }

        private class UpdateGeneralSettingsFixture : UserEndpointFixture
        {
            private GeneralSettingsContract? _contract;
            private Item? _item;
            private ItemCategory? _itemCategory;
            private Store? _store;
            private ShoppingList? _shoppingList;

            public UpdateGeneralSettingsFixture(DockerFixture dockerFixture) : base(dockerFixture)
            {
            }

            public GeneralSetting? ExpectedResult { get; private set; }

            public async Task<IResult> ActAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(_contract);

                var scope = CreateServiceScope();

                return await UserEndpoints.UpdateGeneralSettings(
                    _contract,
                    scope.ServiceProvider.GetRequiredService<ICommandDispatcher>(),
                    scope.ServiceProvider.GetRequiredService<IToContractConverter<IReason, ErrorContract>>(),
                    default);
            }

            public async Task PrepareDatabaseAsync()
            {
                await ApplyMigrationsAsync(ArrangeScope);
            }

            public async Task PrepareDatabaseWithItemAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(_item);
                TestPropertyNotSetException.ThrowIfNull(_itemCategory);
                TestPropertyNotSetException.ThrowIfNull(_store);
                TestPropertyNotSetException.ThrowIfNull(_shoppingList);

                await ApplyMigrationsAsync(ArrangeScope);

                var itemCategoryContext = GetContextInstance<ItemCategoryContext>(ArrangeScope);
                var storeContext = GetContextInstance<StoreContext>(ArrangeScope);
                var itemContext = GetContextInstance<ItemContext>(ArrangeScope);
                var shoppingListContext = GetContextInstance<ShoppingListContext>(ArrangeScope);

                await itemCategoryContext.ItemCategories.AddAsync(_itemCategory);
                await storeContext.Stores.AddAsync(_store);
                await itemContext.Items.AddAsync(_item);
                await shoppingListContext.ShoppingLists.AddAsync(_shoppingList);

                await itemCategoryContext.SaveChangesAsync();
                await storeContext.SaveChangesAsync();
                await itemContext.SaveChangesAsync();
                await shoppingListContext.SaveChangesAsync();
            }

            public void SetupContract()
            {
                _contract = new GeneralSettingsContract(1);
            }

            public void SetupExpectedResult()
            {
                ExpectedResult = new GeneralSetting
                {
                    Id = 0,
                    Currency = 1
                };
            }

            public void SetupItem()
            {
                _item = ItemEntityMother.Initial().WithoutManufacturerId().Create();
                _itemCategory = ItemCategoryEntityMother.Active().WithId(_item.ItemCategoryId!.Value).Create();

                var availability = _item.AvailableAt.First();
                _store = StoreEntityMother.Active().WithId(availability.StoreId)
                    .WithSection(SectionEntityMother.Default().WithId(availability.DefaultSectionId).Create())
                    .Create();
                _shoppingList = ShoppingListEntityMother.Empty()
                    .WithStoreId(availability.StoreId)
                    .Create();
            }

            public void SetupMemoryCache()
            {
                var scope = CreateServiceScope();

                var cache = scope.ServiceProvider.GetRequiredService<IMemoryCache>();
                cache.Set(
                    CacheKeys.GeneralSettings,
                    new Domain.Users.Models.GeneralSetting(new GeneralSettingId(1), Currency.Yen));
            }

            public async Task<IResult> GetSearchResultAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(_item);

                var scope = CreateServiceScope();

                return await ItemEndpoints.SearchItemsForShoppingList(
                    _item.AvailableAt.First().StoreId,
                    _item.Name,
                    scope.ServiceProvider.GetRequiredService<IQueryDispatcher>(),
                    scope.ServiceProvider.GetRequiredService<IToContractConverter<SearchItemForShoppingResultReadModel, SearchItemForShoppingListResultContract>>(),
                    scope.ServiceProvider.GetRequiredService<IToContractConverter<IReason, ErrorContract>>(),
                    default);
            }
        }
    }

    public sealed class GetAllCurrencies(DockerFixture dockerFixture) : IAssemblyFixture<DockerFixture>
    {
        private readonly GetAllCurrenciesFixture _fixture = new(dockerFixture);

        [Fact]
        public async Task GetAllCurrencies_WithValidData_ShouldReturnAllCurrencies()
        {
            // Arrange
            await _fixture.PrepareDatabaseAsync();
            _fixture.SetupExpectedResult();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            result.Should().NotBeNull();

            result.Should().BeOfType<Ok<List<CurrencyContract>>>();
            var okResult = (Ok<List<CurrencyContract>>)result;

            okResult.Value.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        private class GetAllCurrenciesFixture : UserEndpointFixture
        {
            public GetAllCurrenciesFixture(DockerFixture dockerFixture) : base(dockerFixture)
            {
            }

            public IReadOnlyCollection<CurrencyContract>? ExpectedResult { get; private set; }

            public async Task<IResult> ActAsync()
            {
                var scope = CreateServiceScope();
                return await UserEndpoints.GetAllCurrencies(
                    scope.ServiceProvider.GetRequiredService<IQueryDispatcher>(),
                    scope.ServiceProvider.GetRequiredService<IToContractConverter<CurrencyReadModel, CurrencyContract>>(),
                    default);
            }

            public async Task PrepareDatabaseAsync()
            {
                await ApplyMigrationsAsync(ArrangeScope);
            }

            public void SetupExpectedResult()
            {
                ExpectedResult = new List<CurrencyContract>
                {
                    new(0, "€"),
                    new(1, "$"),
                    new(2, "£"),
                    new(3, "¥")
                };
            }

        }
    }

    public sealed class GetGeneralSettings(DockerFixture dockerFixture) : IAssemblyFixture<DockerFixture>
    {
        private readonly GetGeneralSettingsFixture _fixture = new(dockerFixture);

        [Fact]
        public async Task GetGeneralSettings_WithValidData_ShouldReturnGeneralSettings()
        {
            // Arrange
            _fixture.SetupExpectedResult();
            await _fixture.PrepareDatabaseAsync();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            result.Should().NotBeNull();

            result.Should().BeOfType<Ok<Contracts.Users.Queries.GetGeneralSettings.GeneralSettingsContract>>();
            var okResult = (Ok<Contracts.Users.Queries.GetGeneralSettings.GeneralSettingsContract>)result;

            okResult.Value.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        private class GetGeneralSettingsFixture : UserEndpointFixture
        {
            private Contracts.Users.Queries.GetGeneralSettings.CurrencyContract? _expectedCurrency;
            private List<Contracts.Users.Queries.GetGeneralSettings.CurrencyContract> _allCurrencies =
            [
                new(0, "€"),
                new(1, "$"),
                new(2, "£"),
                new(3, "¥")
            ];

            public GetGeneralSettingsFixture(DockerFixture dockerFixture) : base(dockerFixture)
            {
            }

            public Contracts.Users.Queries.GetGeneralSettings.GeneralSettingsContract? ExpectedResult { get; private set; }

            public async Task<IResult> ActAsync()
            {
                var scope = CreateServiceScope();
                return await UserEndpoints.GetGeneralSettings(
                    scope.ServiceProvider.GetRequiredService<IQueryDispatcher>(),
                    scope.ServiceProvider.GetRequiredService<IToContractConverter<IGeneralSetting, Contracts.Users.Queries.GetGeneralSettings.GeneralSettingsContract>>(),
                    default);
            }

            public async Task PrepareDatabaseAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedCurrency);

                await ApplyMigrationsAsync(ArrangeScope);

                var context = GetContextInstance<GeneralSettingContext>(ArrangeScope);

                var settings = await context.GeneralSettings.SingleAsync();
                settings.Currency = _expectedCurrency.Id;

                await context.SaveChangesAsync();
            }

            public void SetupExpectedResult()
            {
                _expectedCurrency = CommonFixture.ChooseRandom(_allCurrencies);
                ExpectedResult = new Contracts.Users.Queries.GetGeneralSettings.GeneralSettingsContract(_expectedCurrency);
            }
        }
    }

    private abstract class UserEndpointFixture : DatabaseFixture
    {
        protected UserEndpointFixture(DockerFixture dockerFixture) : base(dockerFixture)
        {
            ArrangeScope = CreateServiceScope();
        }

        protected readonly IServiceScope ArrangeScope;

        public override IEnumerable<DbContext> GetDbContexts(IServiceScope scope)
        {
            yield return scope.ServiceProvider.GetRequiredService<UserContext>();
            yield return scope.ServiceProvider.GetRequiredService<GeneralSettingContext>();
            yield return scope.ServiceProvider.GetRequiredService<ItemContext>();
            yield return scope.ServiceProvider.GetRequiredService<ItemCategoryContext>();
            yield return scope.ServiceProvider.GetRequiredService<ManufacturerContext>();
            yield return scope.ServiceProvider.GetRequiredService<StoreContext>();
            yield return scope.ServiceProvider.GetRequiredService<ShoppingListContext>();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ArrangeScope.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
