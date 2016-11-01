namespace CaloriesPlan.UTL
{
    public interface IConfigProvider
    {
        string GetConnectionString(string name);
        string GetConfigSettingValue(string key);
        int GetDefaultCaloriesLimit();
    }
}
