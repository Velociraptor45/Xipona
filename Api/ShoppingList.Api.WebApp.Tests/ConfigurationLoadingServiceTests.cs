using FluentAssertions;
using Moq;
using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Core.TestKit.Files;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;
using ProjectHermes.ShoppingList.Api.Vault.Configs;
using ProjectHermes.ShoppingList.Api.Vault.TestKit;
using ProjectHermes.ShoppingList.Api.WebApp.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.WebApp.Tests;

public class ConfigurationLoadingServiceTests
{
    private readonly ConfigurationLoadingServiceFixture _fixture;

    public ConfigurationLoadingServiceTests()
    {
        _fixture = new ConfigurationLoadingServiceFixture();
    }

    [Fact]
    public async Task LoadAsync_WithEnvironmentVariableSet_ShouldLoadFromFile()
    {
        // Arrange
        _fixture.SetupExpectedResult();
        _fixture.SetupEnvironmentVariable();
        _fixture.SetupLoadingConnectionStringFromFile();
        var sut = _fixture.CreateSut();

        // Act
        var result = await sut.LoadAsync();

        // Assert
        result.Should().BeEquivalentTo(_fixture.ExpectedResult);
    }

    [Fact]
    public async Task LoadAsync_WithEnvironmentVariableNotSet_ShouldLoadFromFile()
    {
        // Arrange
        _fixture.SetupExpectedResult();
        _fixture.SetupLoadingConnectionStringFromVault();
        var sut = _fixture.CreateSut();

        // Act
        var result = await sut.LoadAsync();

        // Assert
        result.Should().BeEquivalentTo(_fixture.ExpectedResult);
    }

    private class ConfigurationLoadingServiceFixture
    {
        private readonly FileLoadingServiceMock _fileLoadingServiceMock = new(MockBehavior.Strict);
        private readonly VaultServiceMock _vaultServiceMock = new(MockBehavior.Strict);
        private string? _filePath;

        public ConnectionStrings? ExpectedResult { get; private set; }

        public ConfigurationLoadingService CreateSut()
        {
            return new ConfigurationLoadingService(_fileLoadingServiceMock.Object, _vaultServiceMock.Object);
        }

        public void SetupExpectedResult()
        {
            ExpectedResult = new TestBuilder<ConnectionStrings>().Create();
        }

        public void SetupEnvironmentVariable()
        {
            _filePath = new TestBuilder<string>().Create();
            Environment.SetEnvironmentVariable("PH_SL_DB_CONNECTION_STRING_FILE", _filePath);
        }

        public void SetupLoadingConnectionStringFromFile()
        {
            TestPropertyNotSetException.ThrowIfNull(_filePath);
            TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

            _fileLoadingServiceMock.SetupReadFile(_filePath, ExpectedResult.ShoppingDatabase);
        }

        public void SetupLoadingConnectionStringFromVault()
        {
            TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

            _vaultServiceMock.SetupLoadConnectionStringsAsync(ExpectedResult);
        }
    }
}