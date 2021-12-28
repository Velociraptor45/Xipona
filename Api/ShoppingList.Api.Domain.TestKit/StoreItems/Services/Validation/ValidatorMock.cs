using Moq;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Validation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Services.Validation
{
    public class ValidatorMock : Mock<IValidator>
    {
        public ValidatorMock(MockBehavior behavior) : base(behavior)
        {
        }

        public void SetupValidateAsync(IEnumerable<IStoreItemAvailability> availabilities)
        {
            Setup(m => m.ValidateAsync(availabilities))
                .Returns(Task.CompletedTask);
        }
    }
}