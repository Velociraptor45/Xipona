﻿using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Ports;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Reasons;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Services.Shared;

namespace ProjectHermes.Xipona.Api.Domain.Manufacturers.Services.Queries;

public class ManufacturerQueryService : IManufacturerQueryService
{
    private readonly IManufacturerRepository _manufacturerRepository;

    public ManufacturerQueryService(IManufacturerRepository manufacturerRepository)
    {
        _manufacturerRepository = manufacturerRepository;
    }

    public async Task<IEnumerable<ManufacturerReadModel>> GetAllActiveAsync()
    {
        var manufacturers = await _manufacturerRepository.FindActiveByAsync();

        return manufacturers.Select(m => new ManufacturerReadModel(m));
    }

    public async Task<IEnumerable<ManufacturerSearchResultReadModel>> SearchAsync(string searchInput,
        bool includeDeleted)
    {
        if (string.IsNullOrWhiteSpace(searchInput))
            return Enumerable.Empty<ManufacturerSearchResultReadModel>();

        var manufacturerModels = await _manufacturerRepository.FindByAsync(searchInput, includeDeleted);

        return manufacturerModels.Select(model => new ManufacturerSearchResultReadModel(model.Id, model.Name));
    }

    public async Task<IManufacturer> GetAsync(ManufacturerId manufacturerId)
    {
        var manufacturer = await _manufacturerRepository.FindActiveByAsync(manufacturerId);

        if (manufacturer is null)
            throw new DomainException(new ManufacturerNotFoundReason(manufacturerId));

        return manufacturer;
    }
}