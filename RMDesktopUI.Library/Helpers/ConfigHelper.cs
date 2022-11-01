using System.Configuration;

namespace RMDesktopUI.Library.Helpers
{
    // The ConfigHelper class is used to read data from the UI's App.ccnfig's <appSettings>
    // and would be registered as a singleton for the sole purpose of accessing this data
    public class ConfigHelper : IConfigHelper
    {
        public decimal GetTaxRate()
        {
            string rateText = ConfigurationManager.AppSettings["taxRate"];

            bool IsValidTaxRate = decimal.TryParse(rateText, out decimal output);

            if (!IsValidTaxRate)
                throw new ConfigurationErrorsException("The taxRate is not set up properly");

            return output;
        }

    }
}
