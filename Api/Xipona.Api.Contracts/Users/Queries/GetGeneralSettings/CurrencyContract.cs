namespace ProjectHermes.Xipona.Api.Contracts.Users.Queries.GetGeneralSettings
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
        public CurrencyContract(int id, string symbol)
        {
            Id = id;
            Symbol = symbol;
        }

        /// <summary>
        /// The unique identifier of the currency.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The symbol of the currency.
        /// </summary>
        public string Symbol { get; set; }
    }
}
