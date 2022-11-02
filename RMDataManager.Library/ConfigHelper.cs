using System.Configuration;

namespace RMDataManager.Library
{
    public static class ConfigHelper
    {
        public static decimal GetTaxRate()
        {
            string rateText = ConfigurationManager.AppSettings["taxRate"];

            bool IsValidTaxRate = decimal.TryParse(rateText, out decimal output);

            if (!IsValidTaxRate)
                throw new ConfigurationErrorsException("The taxRate is not set up properly");

            return output;
        }
    }
}
