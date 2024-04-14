using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Shared;

namespace ProjectHermes.Xipona.Api.ApplicationServices.ItemCategories.Queries.AllActiveItemCategories;

public class AllActiveItemCategoriesQuery : IQuery<IEnumerable<ItemCategoryReadModel>>
{
}