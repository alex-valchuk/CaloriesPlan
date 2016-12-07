using System.Configuration;

using CaloriesPlan.UTL.Config.Abstractions;

namespace CaloriesPlan.UTL.Config
{
    public class DesktopConfigProvider : IConfigProvider
    {
        public string GetConfigSettingValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;            
        }

        public int GetDefaultCaloriesLimit()
        {
            int value;
            var strValue = this.GetConfigSettingValue("DefaultCaloriesLimit");
            int.TryParse(strValue, out value);

            return value;
        }
    }
}
