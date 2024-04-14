﻿using ProjectHermes.Xipona.Api.Domain.Items.Models;

namespace ProjectHermes.Xipona.Api.Domain.Items.Services.Queries;

public interface IItemQueryService
{
    Task<ItemReadModel> GetAsync(ItemId itemId);
}