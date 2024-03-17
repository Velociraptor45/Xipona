using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Core.Services;
using ProjectHermes.Xipona.Api.Core.Tests.Converter;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Models.Factories;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Models;
using ProjectHermes.Xipona.Api.Repositories.Items.Converters.ToDomain;
using ItemType = ProjectHermes.Xipona.Api.Repositories.Items.Entities.ItemType;

namespace ProjectHermes.Xipona.Api.Repositories.Tests.Converters.ToDomain;

public class ItemTypeConverterTests : ToDomainConverterTestBase<ItemType, IItemType>
{
    protected override (ItemType, IItemType) CreateTestObjects()
    {
        var destination = new ItemTypeBuilder().Create();
        var source = GetSource(destination);

        return (source, destination);
    }

    public static ItemType GetSource(IItemType destination)
    {
        var availabilities = destination.Availabilities
            .Select(ItemTypeAvailabilityConverterTests.GetSource)
            .ToList();

        return new ItemType
        {
            Id = destination.Id,
            Name = destination.Name,
            AvailableAt = availabilities,
            PredecessorId = destination.PredecessorId,
            IsDeleted = destination.IsDeleted,
            CreatedAt = destination.CreatedAt
        };
    }

    protected override void SetupServiceCollection()
    {
        AddDependencies(ServiceCollection);
    }

    public static void AddDependencies(IServiceCollection serviceCollection)
    {
        serviceCollection.AddImplementationOfGenericType(typeof(ItemTypeConverter).Assembly, typeof(IToDomainConverter<,>));
        serviceCollection.AddImplementationOfNonGenericType(typeof(IItemTypeFactory).Assembly, typeof(IItemTypeFactory));
        serviceCollection.AddTransient<IDateTimeService, DateTimeService>();
    }
}