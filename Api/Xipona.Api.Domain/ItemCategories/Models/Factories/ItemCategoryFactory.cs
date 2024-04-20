using ProjectHermes.Xipona.Api.Core.Services;

namespace ProjectHermes.Xipona.Api.Domain.ItemCategories.Models.Factories;

public class ItemCategoryFactory : IItemCategoryFactory
{
    private readonly IDateTimeService _dateTimeService;

    public ItemCategoryFactory(IDateTimeService dateTimeService)
    {
        _dateTimeService = dateTimeService;
    }

    public IItemCategory Create(ItemCategoryId id, ItemCategoryName name, bool isDeleted, DateTimeOffset createdAt)
    {
        return new ItemCategory(id, name, isDeleted, createdAt);
    }

    public IItemCategory CreateNew(ItemCategoryName name)
    {
        return new ItemCategory(ItemCategoryId.New, name, false, _dateTimeService.UtcNow);
    }
}