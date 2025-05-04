namespace ProjectHermes.Xipona.Api.Contracts.Users.Commands.UpdateGeneralSettings
{
    /// <summary>
    /// Represents the contract for updating application-wide settings.
    /// </summary>
    public class GeneralSettingsContract
    {
        /// <summary> 
        /// </summary>
        /// <param name="currencyId"></param>
        public GeneralSettingsContract(int currencyId)
        {
            CurrencyId = currencyId;
        }

        /// <summary>
        /// The currency in which the application will display prices.
        /// </summary>
        public int CurrencyId { get; set; }
    }
}
