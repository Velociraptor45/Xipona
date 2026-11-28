using Xipona.Api.ApplicationServices.ItemCategories.Commands.DeleteItemCategory;
using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.ItemCategories.Models;

namespace Xipona.Api.Endpoint.v1.Converters.ToDomain.ItemCategories;

public class DeleteItemCategoryCommandConverter : IToDomainConverter<Guid, DeleteItemCategoryCommand>
{
    public DeleteItemCategoryCommand ToDomain(Guid source)
    {
        return new DeleteItemCategoryCommand(new ItemCategoryId(source));
    }
}