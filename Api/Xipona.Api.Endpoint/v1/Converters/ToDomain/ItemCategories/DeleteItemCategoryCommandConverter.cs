using ProjectHermes.Xipona.Api.ApplicationServices.ItemCategories.Commands.DeleteItemCategory;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToDomain.ItemCategories;

public class DeleteItemCategoryCommandConverter : IToDomainConverter<Guid, DeleteItemCategoryCommand>
{
    public DeleteItemCategoryCommand ToDomain(Guid source)
    {
        return new DeleteItemCategoryCommand(new ItemCategoryId(source));
    }
}