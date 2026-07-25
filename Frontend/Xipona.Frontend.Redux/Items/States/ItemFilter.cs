using Xipona.Frontend.Redux.Items.States.Filters;

namespace Xipona.Frontend.Redux.Items.States;

public record ItemFilter(
    ItemStore? SelectedStore,
    ItemCategoryFilter ItemCategoryFilter,
    ManufacturerFilter ManufacturerFilter,
    bool IsLoadButtonActive);