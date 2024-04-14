using FluentAssertions;
using Microsoft.Extensions.Configuration;
using ProjectHermes.Xipona.Api.Core.TestKit;
using ProjectHermes.Xipona.Api.Core.TestKit.Files;
using ProjectHermes.Xipona.Api.Repositories.Common.Services;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using ProjectHermes.Xipona.Api.Vault.Configs;
using ProjectHermes.Xipona.Api.Vault.TestKit;
using System;
using Xunit;

namespace ProjectHermes.Xipona.Api.Repositories.Tests.Common.Services;

public class ConfigurationLoadingServiceTests
{
    private readonly ConfigurationLoadingServiceFixture _fixture = new();

    [Fact]
    public async Task LoadAsync_WithEnvironmentVariableSet_ShouldLoadFromFile()
    {
        // Arrange
        _fixture.SetupEnvironmentVariables();
        _fixture.SetupGettingConfigurationFromAppsettings();
        _fixture.SetupExpectedResult();
        _fixture.SetupLoadingUsernameFromFile();
        _fixture.SetupLoadingPasswordFromFile();
        var sut = _fixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_fixture.ConfigurationMock);

        // Act
        var result = await sut.LoadAsync(_fixture.ConfigurationMock);

        // Assert
        result.Should().BeEquivalentTo(_fixture.ExpectedResult);
    }

    [Fact]
    public async Task LoadAsync_WithEnvironmentVariableSet_WithDatabaseSectionMissing_ShouldThrow()
    {
        // Arrange
        _fixture.SetupEnvironmentVariables();
        _fixture.SetupGettingEmptyConfigurationFromAppsettings();
        var sut = _fixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_fixture.ConfigurationMock);

        // Act
        var func = async () => await sut.LoadAsync(_fixture.ConfigurationMock);

        // Assert
        await func.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task LoadAsync_WithUsernameEnvironmentVariableNotSet_ShouldLoadFromVault()
    {
        // Arrange
        _fixture.SetupUsernameEnvironmentVariableEmpty();
        _fixture.SetupGettingConfigurationFromAppsettings();
        _fixture.SetupExpectedResult();
        _fixture.SetupLoadingCredentialsFromVault();
        var sut = _fixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_fixture.ConfigurationMock);

        // Act
        var result = await sut.LoadAsync(_fixture.ConfigurationMock);

        // Assert
        result.Should().BeEquivalentTo(_fixture.ExpectedResult);
    }

    [Fact]
    public async Task LoadAsync_WithPasswordEnvironmentVariableNotSet_ShouldLoadFromVault()
    {
        // Arrange
        _fixture.SetupPasswordEnvironmentVariableEmpty();
        _fixture.SetupGettingConfigurationFromAppsettings();
        _fixture.SetupExpectedResult();
        _fixture.SetupLoadingCredentialsFromVault();
        var sut = _fixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_fixture.ConfigurationMock);

        // Act
        var result = await sut.LoadAsync(_fixture.ConfigurationMock);

        // Assert
        result.Should().BeEquivalentTo(_fixture.ExpectedResult);
    }

    private class ConfigurationLoadingServiceFixture
    {
        private readonly FileLoadingServiceMock _fileLoadingServiceMock = new(MockBehavior.Strict);
        private readonly VaultServiceMock _vaultServiceMock = new(MockBehavior.Strict);
        private string? _usernamePath;
        private string? _passwordPath;
        private DatabaseConfigurationLoadingService.DatabaseConfig? _dbConfig;
        private string? _username;
        private string? _password;

        public ConnectionStrings? ExpectedResult { get; private set; }
        public IConfiguration? ConfigurationMock { get; private set; }

        public DatabaseConfigurationLoadingService CreateSut()
        {
            return new DatabaseConfigurationLoadingService(_fileLoadingServiceMock.Object, _vaultServiceMock.Object);
        }

        public void SetupExpectedResult()
        {
            TestPropertyNotSetException.ThrowIfNull(_dbConfig);

            _username = new TestBuilder<string>().Create();
            _password = new TestBuilder<string>().Create();

            ExpectedResult = new ConnectionStrings()
            {
                ShoppingDatabase = $"server={_dbConfig.Address};port={_dbConfig.Port};database={_dbConfig.Name};user id={_username};pwd={_password};AllowUserVariables=true;UseAffectedRows=false"
            };
        }

        public void SetupGettingConfigurationFromAppsettings()
        {
            _dbConfig = new TestBuilder<DatabaseConfigurationLoadingService.DatabaseConfig>().Create();

            var inMemorySettings = new Dictionary<string, string>()
            {
                { "Database:Name", _dbConfig.Name },
                { "Database:Address", _dbConfig.Address },
                { "Database:Port", _dbConfig.Port },
            };

            ConfigurationMock = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddInMemoryCollection(inMemorySettings!)
                .Build();
        }

        public void SetupGettingEmptyConfigurationFromAppsettings()
        {
            _dbConfig = new TestBuilder<DatabaseConfigurationLoadingService.DatabaseConfig>().Create();

            var inMemorySettings = new Dictionary<string, string>();

            ConfigurationMock = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddInMemoryCollection(inMemorySettings!)
                .Build();
        }

        public void SetupEnvironmentVariables()
        {
            _usernamePath = new TestBuilder<string>().Create();
            _passwordPath = new TestBuilder<string>().Create();
            Environment.SetEnvironmentVariable("PH_XIPONA_DB_USERNAME_FILE", _usernamePath);
            Environment.SetEnvironmentVariable("PH_XIPONA_DB_PASSWORD_FILE", _passwordPath);
        }

        public void SetupUsernameEnvironmentVariableEmpty()
        {
            Environment.SetEnvironmentVariable("PH_XIPONA_DB_USERNAME_FILE", string.Empty);
            _passwordPath = new TestBuilder<string>().Create();
            Environment.SetEnvironmentVariable("PH_XIPONA_DB_PASSWORD_FILE", _passwordPath);
        }

        public void SetupPasswordEnvironmentVariableEmpty()
        {
            Environment.SetEnvironmentVariable("PH_XIPONA_DB_PASSWORD_FILE", string.Empty);
            _usernamePath = new TestBuilder<string>().Create();
            Environment.SetEnvironmentVariable("PH_XIPONA_DB_USERNAME_FILE", _usernamePath);
        }

        public void SetupLoadingUsernameFromFile()
        {
            TestPropertyNotSetException.ThrowIfNull(_usernamePath);
            TestPropertyNotSetException.ThrowIfNull(_username);

            _fileLoadingServiceMock.SetupReadFile(_usernamePath, _username);
        }

        public void SetupLoadingPasswordFromFile()
        {
            TestPropertyNotSetException.ThrowIfNull(_passwordPath);
            TestPropertyNotSetException.ThrowIfNull(_password);

            _fileLoadingServiceMock.SetupReadFile(_passwordPath, _password);
        }

        public void SetupLoadingCredentialsFromVault()
        {
            TestPropertyNotSetException.ThrowIfNull(_username);
            TestPropertyNotSetException.ThrowIfNull(_password);

            _vaultServiceMock.SetupLoadCredentialsAsync((_username, _password));
        }
    }
}