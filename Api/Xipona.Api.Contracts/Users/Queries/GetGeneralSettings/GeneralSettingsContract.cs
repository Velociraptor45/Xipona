namespace Xipona.Api.Contracts.Users.Queries.GetGeneralSettings
{
    /// <summary>
    /// Represents the general settings of the application
    /// </summary>
    public class GeneralSettingsContract
    {
        /// <summary> 
        /// </summary>
        /// <param name="currency"></param>
        public GeneralSettingsContract(CurrencyContract currency)
        {
            Currency = currency;
        }

        /// <summary>
        /// The currency in which prices for the application are interpreted.
        /// </summary>
        public CurrencyContract Currency { get; }
    }
}
