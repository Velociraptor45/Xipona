using ProjectHermes.Xipona.Api.Contracts.Items.Queries.Get;

namespace ProjectHermes.Xipona.Api.Contracts.TestKit.Items.Queries.Get;

public static class ItemTypeContractMother
{
    public static ItemTypeContractBuilder Valid()
    {
        return new ItemTypeContractBuilder()
            .WithAvailabilities(new List<ItemAvailabilityContract>()
            {
                ItemAvailabilityContractMother.Valid().Create(),
                ItemAvailabilityContractMother.Valid().Create()
            });
    }
}