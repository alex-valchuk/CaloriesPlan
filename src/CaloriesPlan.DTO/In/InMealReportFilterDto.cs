using System;

namespace CaloriesPlan.DTO.In
{
    public class InMealReportFilterDto
    {
        private DateTime? dateFrom;
        private DateTime? dateTo;
        private DateTime? timeFrom;
        private DateTime? timeTo;

        public DateTime? DateFrom
        {
            get
            {
                if (this.dateFrom != null)
                    return this.dateFrom.Value.ToUniversalTime();

                return this.dateFrom;
            }
            set { this.dateFrom = value; }
        }

        public DateTime? DateTo
        {
            get
            {
                if (this.dateTo != null)
                    return this.dateTo.Value.ToUniversalTime();

                return this.dateTo;
            }
            set { this.dateTo = value; }
        }

        public DateTime? TimeFrom
        {
            get
            {
                if (this.timeFrom != null)
                    return this.timeFrom.Value.ToUniversalTime();

                return this.timeFrom;
            }
            set { this.timeFrom = value; }
        }

        public DateTime? TimeTo
        {
            get
            {
                if (this.timeTo != null)
                    return this.timeTo.Value.ToUniversalTime();

                return this.timeTo;
            }
            set { this.timeTo = value; }
        }
    }
}
