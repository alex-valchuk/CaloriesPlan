namespace CaloriesPlan.DAL.DataModel.Abstractions
{
    public interface IUser
    {
        string Id { get; set; }
        string UserName { get; set; }
        int DailyCaloriesLimit { get; set; }
    }
}
