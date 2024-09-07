using ProjectHermes.Xipona.Api.ApplicationServices.ItemCategories.Commands.CreateItemCategory;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToDomain.ItemCategories;

public class CreateItemCategoryCommandConverter : IToDomainConverter<string, CreateItemCategoryCommand>
{
    public CreateItemCategoryCommand ToDomain(string source)
    {
        return new CreateItemCategoryCommand(new ItemCategoryName(source));
    }
}