using Xipona.Api.ApplicationServices.ItemCategories.Commands.CreateItemCategory;
using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.ItemCategories.Models;

namespace Xipona.Api.Endpoint.v1.Converters.ToDomain.ItemCategories;

public class CreateItemCategoryCommandConverter : IToDomainConverter<string, CreateItemCategoryCommand>
{
    public CreateItemCategoryCommand ToDomain(string source)
    {
        return new CreateItemCategoryCommand(new ItemCategoryName(source));
    }
}