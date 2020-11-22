namespace ShoppingList.Api.Domain.Commands.CreateManufacturer
{
    public class CreateManufacturerCommand : ICommand<bool>
    {
        public CreateManufacturerCommand(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new System.ArgumentException($"'{nameof(name)}' cannot be null or whitespace", nameof(name));
            }

            Name = name;
        }

        public string Name { get; }
    }
}