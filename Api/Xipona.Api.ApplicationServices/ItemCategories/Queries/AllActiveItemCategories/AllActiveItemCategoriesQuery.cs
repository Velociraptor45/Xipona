using Xipona.Api.ApplicationServices.Common.Queries;
using Xipona.Api.Domain.ItemCategories.Services.Shared;

namespace Xipona.Api.ApplicationServices.ItemCategories.Queries.AllActiveItemCategories;

public class AllActiveItemCategoriesQuery : IQuery<IEnumerable<ItemCategoryReadModel>>
{
}