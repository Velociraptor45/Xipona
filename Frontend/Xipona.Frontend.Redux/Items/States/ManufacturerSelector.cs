using ProjectHermes.Xipona.Frontend.Redux.Manufacturers.States;

namespace ProjectHermes.Xipona.Frontend.Redux.Items.States;
public record ManufacturerSelector(IReadOnlyCollection<ManufacturerSearchResult> Manufacturers, string Input);