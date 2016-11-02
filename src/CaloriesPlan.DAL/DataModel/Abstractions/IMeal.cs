using System;

namespace CaloriesPlan.DAL.DataModel.Abstractions
{
    public interface IMeal
    {
        int ID { get; set; }
        string Text { get; set; }
        int Calories { get; set; }
        DateTime EatingDate { get; set; }
        string UserID { get; set; }
    }
}
