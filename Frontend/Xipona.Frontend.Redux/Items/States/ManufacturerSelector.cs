using Xipona.Frontend.Redux.Manufacturers.States;

namespace Xipona.Frontend.Redux.Items.States;
public record ManufacturerSelector(IReadOnlyCollection<ManufacturerSearchResult> Manufacturers, string Input);