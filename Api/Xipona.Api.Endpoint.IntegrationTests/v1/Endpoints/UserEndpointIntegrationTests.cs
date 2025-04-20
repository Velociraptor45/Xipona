using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Contracts.Common;
using ProjectHermes.Xipona.Api.Contracts.Users.Commands.Login;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Endpoint.v1.Endpoints;
using ProjectHermes.Xipona.Api.Repositories.Users.Contexts;
using ProjectHermes.Xipona.Api.Repositories.Users.Entities;
using ProjectHermes.Xipona.Api.TestTools.AutoFixture;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using ProjectHermes.Xipona.Api.WebApp.Auth;
using System;
using System.IdentityModel.Tokens.Jwt;
using Xunit;

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
                ctx.Request.Headers["Authorization"] = "Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIwMDVjNDJkYy0zNTI5LTQwODYtOTY4OS1kMjNiNjNkYTYzY2EiLCJnaXZlbl9uYW1lIjoiSm9obi1vaCBEZWVyIn0.CEBWi9GOoMZKC0Lu_qSXbCGbbv7UlCSeR_ttPAC1N-YVd2RXnHVAKe0BLhvgRdB8jMY0LXse2LrODcKtC0eLvFjMCg6TWGSuS1cM-QP-yrHRPpzvNNRZojkSSfeTQcmo475dSCcNpqMtJhvYj0d6bAuunFv0vm5yL3tWRNgtDKPUvsx8DkbTcq1-r6F8K7LCxUWjWO6mj-Vva0AbkJQIgO32e3j7Ny2ArykTpvFG00-5uVxc9po7OMIEw7ld23PFwqkrAD9Z9plxXK9GTTNHj-4sh0P-MqrONrXOlER5Bqx6lzis0freHPV_NspXXoMGK599S9eFG2CPsU7pR-SXjg";

                return await UserEndpoints.Login(
                    ctx,
                    new JwtSecurityTokenHandler(),
                    scope.ServiceProvider.GetRequiredService<ICommandDispatcher>(),
                    scope.ServiceProvider.GetRequiredService<IToContractConverter<IReason, ErrorContract>>(),
                    new AuthenticationOptions()
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
                ExpectedUser = new User()
                {
                    Id = Guid.Parse("005c42dc-3529-4086-9689-d23b63da63ca"),
                    CreatedAt = DateTimeOffset.UtcNow,
                };
            }

            public void SetupExpectedResult()
            {
                ExpectedResult = new UserInfoContract()
                {
                    DisplayName = "John-oh Deer"
                };
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
