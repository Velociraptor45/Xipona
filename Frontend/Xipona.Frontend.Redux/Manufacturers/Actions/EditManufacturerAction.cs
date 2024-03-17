using ProjectHermes.Xipona.Frontend.Redux.Shared.Actions;

namespace ProjectHermes.Xipona.Frontend.Redux.Manufacturers.Actions;

public record EditManufacturerAction : ISearchResultTriggerAction
{
    public Guid Id { get; init; }
}