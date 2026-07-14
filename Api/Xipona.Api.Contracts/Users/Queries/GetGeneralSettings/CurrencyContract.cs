namespace Xipona.Api.Contracts.Users.Queries.GetGeneralSettings
{
    /// <summary>
    /// Represents a currency.
    /// </summary>
    public class CurrencyContract
    {
        /// <summary> 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="symbol"></param>
        /// <param name="isTrailing"></param>
        public CurrencyContract(int id, string symbol, bool isTrailing)
        {
            Id = id;
            Symbol = symbol;
            IsTrailing = isTrailing;
        }

        /// <summary>
        /// The unique identifier of the currency.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The symbol of the currency.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Whether the currency symbol is placed before the amount or after the amount.
        /// </summary>
        public bool IsTrailing { get; }
    }
}
