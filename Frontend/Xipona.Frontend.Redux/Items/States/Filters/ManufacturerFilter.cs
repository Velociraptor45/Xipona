
using Xipona.Frontend.Redux.Manufacturers.States;

namespace Xipona.Frontend.Redux.Items.States.Filters;

public record ManufacturerFilter(IReadOnlyList<ManufacturerSearchResult> Manufacturers, string Input,
    ManufacturerSearchResult? SelectedManufacturer);