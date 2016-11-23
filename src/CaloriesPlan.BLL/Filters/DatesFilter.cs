using System;

namespace CaloriesPlan.BLL.Filters
{
    public class DatesFilter
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public DateTime TimeFrom { get; set; }
        public DateTime TimeTo { get; set; }
    }
}
