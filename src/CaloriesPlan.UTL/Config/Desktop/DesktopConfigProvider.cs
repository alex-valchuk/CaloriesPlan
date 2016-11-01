using System.Configuration;

namespace CaloriesPlan.UTL.Config.Desktop
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
