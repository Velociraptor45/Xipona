using ShoppingList.EntityModels;

namespace ShoppingList.Util
{
    public static class LabelTranslator
    {

        public static string GetQuantityPriceLabel(QuantityType quantityType) =>
            quantityType switch
            {
                QuantityType.Unit => "",
                QuantityType.Weight => "/kg",
                QuantityType.Fluid => "/l",
                _ => ""
            };

        public static string GetQuantityLabel(QuantityType quantityType) =>
            quantityType switch
            {
                QuantityType.Unit => "x",
                QuantityType.Weight => "g",
                QuantityType.Fluid => "ml",
                _ => ""
            };
    }
}
