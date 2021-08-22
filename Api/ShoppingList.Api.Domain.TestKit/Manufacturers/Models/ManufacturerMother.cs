namespace ShoppingList.Api.Domain.TestKit.Manufacturers.Models
{
    public static class ManufacturerMother
    {
        public static ManufacturerBuilder Deleted()
        {
            return new ManufacturerBuilder()
                .WithIsDeleted(true);
        }

        public static ManufacturerBuilder NotDeleted()
        {
            return new ManufacturerBuilder()
                .WithIsDeleted(false);
        }
    }
}