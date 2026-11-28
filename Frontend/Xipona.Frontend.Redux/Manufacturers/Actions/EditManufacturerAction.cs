using Xipona.Frontend.Redux.Shared.Actions;

namespace Xipona.Frontend.Redux.Manufacturers.Actions;

public record EditManufacturerAction : ISearchResultTriggerAction
{
    public Guid Id { get; init; }
}