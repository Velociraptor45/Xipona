﻿using ProjectHermes.ShoppingList.Api.Core.Files;
using ShoppingList.Api.Vault;
using ShoppingList.Api.Vault.Configs;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.WebApp.Services;

public class ConfigurationLoadingService
{
    private readonly IFileLoadingService _fileLoadingService;
    private readonly IVaultService _vaultService;

    public ConfigurationLoadingService(IFileLoadingService fileLoadingService, IVaultService vaultService)
    {
        _fileLoadingService = fileLoadingService;
        _vaultService = vaultService;
    }

    public async Task<ConnectionStrings> LoadAsync()
    {
        var connectionStringFile = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING_FILE");

        if (connectionStringFile is null)
        {
            return await _vaultService.LoadConnectionStringsAsync();
        }

        var connectionString = _fileLoadingService.ReadFile(connectionStringFile);
        return new ConnectionStrings { ShoppingDatabase = connectionString };
    }
}