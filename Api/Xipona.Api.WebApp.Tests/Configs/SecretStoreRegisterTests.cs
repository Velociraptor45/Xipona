using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ProjectHermes.Xipona.Api.Core.TestKit;
using ProjectHermes.Xipona.Api.Core.TestKit.Files;
using ProjectHermes.Xipona.Api.Secrets;
using ProjectHermes.Xipona.Api.Secrets.Vault;
using ProjectHermes.Xipona.Api.Secrets.Vault.Config;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.WebApp.Configs;

public class SecretStoreRegisterTests
{
    private readonly SecretRetrieverFixture _fixture = new();

    [Fact]
    public void RegisterSecretStore_WithVaultUsernameAndPassword_ShouldRegisterVaultAsSecretStore()
    {
        // Arrange
        _fixture.SetupVaultPassword();
        _fixture.SetupVaultUsername();
        _fixture.SetupVaultConfig();
        _fixture.SetupConfiguration();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Configuration);

        // Act
        SecretStoreRegister.RegisterSecretStore(_fixture.Configuration, _fixture.FileLoadingServiceMock.Object,
            _fixture.Services);

        // Assert
        _fixture.VerifyVaultDependencies();
    }

    [Fact]
    public void RegisterSecretStore_WithVaultUsernameFileAndPasswordFile_ShouldRegisterVaultAsSecretStore()
    {
        // Arrange
        _fixture.SetupVaultPasswordFile();
        _fixture.SetupVaultUsernameFile();
        _fixture.SetupVaultConfig();
        _fixture.SetupConfiguration();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Configuration);

        // Act
        SecretStoreRegister.RegisterSecretStore(_fixture.Configuration, _fixture.FileLoadingServiceMock.Object,
            _fixture.Services);

        // Assert
        _fixture.VerifyVaultDependencies();
    }

    [Fact]
    public void RegisterSecretStore_WithVaultUsernameFileAndPassword_ShouldRegisterVaultAsSecretStore()
    {
        // Arrange
        _fixture.SetupVaultPassword();
        _fixture.SetupVaultUsernameFile();
        _fixture.SetupVaultConfig();
        _fixture.SetupConfiguration();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Configuration);

        // Act
        SecretStoreRegister.RegisterSecretStore(_fixture.Configuration, _fixture.FileLoadingServiceMock.Object,
            _fixture.Services);

        // Assert
        _fixture.VerifyVaultDependencies();
    }

    [Fact]
    public void RegisterSecretStore_WithVaultUsernameAndPasswordFile_ShouldRegisterVaultAsSecretStore()
    {
        // Arrange
        _fixture.SetupVaultPasswordFile();
        _fixture.SetupVaultUsername();
        _fixture.SetupVaultConfig();
        _fixture.SetupConfiguration();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Configuration);

        // Act
        SecretStoreRegister.RegisterSecretStore(_fixture.Configuration, _fixture.FileLoadingServiceMock.Object,
            _fixture.Services);

        // Assert
        _fixture.VerifyVaultDependencies();
    }

    [Fact]
    public void RegisterSecretStore_WithVaultUsernameAndNoPassword_ShouldRegisterEnvAsSecretStore()
    {
        // Arrange
        _fixture.SetupVaultUsername();
        _fixture.SetupVaultConfig();
        _fixture.SetupConfiguration();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Configuration);

        // Act
        SecretStoreRegister.RegisterSecretStore(_fixture.Configuration, _fixture.FileLoadingServiceMock.Object,
            _fixture.Services);

        // Assert
        _fixture.VerifyEnvDependencies();
    }

    [Fact]
    public void RegisterSecretStore_WithVaultPasswordAndNoUsername_ShouldRegisterEnvAsSecretStore()
    {
        // Arrange
        _fixture.SetupVaultPassword();
        _fixture.SetupVaultConfig();
        _fixture.SetupConfiguration();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Configuration);

        // Act
        SecretStoreRegister.RegisterSecretStore(_fixture.Configuration, _fixture.FileLoadingServiceMock.Object,
            _fixture.Services);

        // Assert
        _fixture.VerifyEnvDependencies();
    }

    [Fact]
    public void RegisterSecretStore_WithNoVaultPasswordOrUsername_ShouldRegisterEnvAsSecretStore()
    {
        // Arrange
        _fixture.SetupVaultConfig();
        _fixture.SetupConfiguration();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Configuration);

        // Act
        SecretStoreRegister.RegisterSecretStore(_fixture.Configuration, _fixture.FileLoadingServiceMock.Object,
            _fixture.Services);

        // Assert
        _fixture.VerifyEnvDependencies();
    }

    private sealed class SecretRetrieverFixture
    {
        private readonly string _username = new TestBuilder<string>().Create();
        private readonly string _password = new TestBuilder<string>().Create();
        private string? _vaultUsername;
        private string? _vaultUsernameFile;
        private string? _vaultPassword;
        private string? _vaultPasswordFile;
        private readonly List<KeyValuePair<string, string?>> _collection = [];
        private readonly string _uri = $"https://sub{new TestBuilder<int>().Create()}domain.vault.com/";

        public IServiceCollection Services { get; }
        public IConfiguration? Configuration { get; private set; }
        public FileLoadingServiceMock FileLoadingServiceMock { get; } = new(MockBehavior.Strict);

        public SecretRetrieverFixture()
        {
            Services = new ServiceCollection();
            Services.AddSingleton(FileLoadingServiceMock.Object);
        }

        public void SetupVaultUsername()
        {
            _vaultUsername = _username;
            _collection.Add(new("PH_XIPONA_VAULT_USERNAME", _vaultUsername));
        }

        public void SetupVaultUsernameFile()
        {
            _vaultUsernameFile = new TestBuilder<string>().Create();
            FileLoadingServiceMock.SetupReadFile(_vaultUsernameFile, _username);
            _collection.Add(new("PH_XIPONA_VAULT_USERNAME_FILE", _vaultUsernameFile));
        }

        public void SetupVaultPassword()
        {
            _vaultPassword = _password;
            _collection.Add(new("PH_XIPONA_VAULT_PASSWORD", _vaultPassword));
        }

        public void SetupVaultPasswordFile()
        {
            _vaultPasswordFile = new TestBuilder<string>().Create();
            FileLoadingServiceMock.SetupReadFile(_vaultPasswordFile, _password);
            _collection.Add(new("PH_XIPONA_VAULT_PASSWORD_FILE", _vaultPasswordFile));
        }

        public void SetupVaultConfig()
        {
            _collection.Add(new("KeyVault:Uri", _uri));
        }

        public void SetupConfiguration()
        {
            Configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(_collection)
                .Build();
            Services.AddSingleton(Configuration);
        }

        public void VerifyVaultDependencies()
        {
            var provider = Services.BuildServiceProvider();
            var secretStore = provider.GetRequiredService<ISecretStore>();
            secretStore.Should().BeOfType<VaultService>();

            var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
            using var httpClient = httpClientFactory.CreateClient("vault");
            httpClient.Should().NotBe(null);
            httpClient.BaseAddress.Should().Be(new Uri(_uri));
            provider.GetRequiredService<VaultConfig>().Should().NotBeNull();
            provider.GetRequiredService<VaultCredentials>().Should().NotBeNull();
        }

        public void VerifyEnvDependencies()
        {
            var provider = Services.BuildServiceProvider();
            var secretStore = provider.GetRequiredService<ISecretStore>();
            secretStore.Should().BeOfType<EnvSecretStore>();
        }
    }
}
